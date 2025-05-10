import os
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.vectorstores import FAISS
from langchain.embeddings import OpenAIEmbeddings
from dotenv import load_dotenv

# Carregar variáveis de ambiente
load_dotenv()
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY")

# Caminhos dos arquivos
DATA_FILE = "data/knowledge.md"
INDEX_DIR = "embeddings/faiss_index"

def update_embeddings():
    """Read the file knowledge.md, generate embeddings and save in FAISS."""
    if not OPENAI_API_KEY:
        raise ValueError("❌ OPENAI_API_KEY not configured. Check file .env.")

    # Ler o conteúdo do arquivo
    with open(DATA_FILE, "r", encoding="utf-8") as f:
        text = f.read()

    # Dividir o texto em partes menores
    text_splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    texts = text_splitter.split_text(text)

    # Gerar embeddings
    embeddings = OpenAIEmbeddings()
    vector_store = FAISS.from_texts(texts, embeddings)

    # Salvar o índice FAISS
    vector_store.save_local(INDEX_DIR)
    print("✅ Embeddings updated sucessfully!")

if __name__ == "__main__":
    update_embeddings()
