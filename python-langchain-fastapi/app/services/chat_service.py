import os
from langchain.vectorstores import FAISS
from langchain.embeddings import OpenAIEmbeddings
from langchain.chat_models import ChatOpenAI
from langchain.chains import ConversationalRetrievalChain
from langchain.memory import ConversationBufferMemory
from langchain.prompts import PromptTemplate
from app.modules.config import OPENAI_API_KEY, OPENAI_API_MODEL

# Caminho dos embeddings
BASE_DIR = os.path.dirname(os.path.abspath(__file__))
INDEX_DIR = os.path.join(BASE_DIR, "../../embeddings/faiss_index")

class ChatService:
    def __init__(self):
        print("✅ ChatService foi inicializado!")
        """Start the service loading the embeddings and configuring the AI."""
        if not OPENAI_API_KEY:
            raise ValueError("❌ OPENAI_API_KEY not configured. Check file .env.")

        try:
            self.embeddings = OpenAIEmbeddings()
            self.vector_store = FAISS.load_local(INDEX_DIR, self.embeddings, allow_dangerous_deserialization=True)

            print("🔍 Embeddings carregados com sucesso {vector_store}.")

            # Criar memória da conversa
            self.memory = ConversationBufferMemory(memory_key="chat_history", return_messages=True)

            # Configurar modelo de IA
            self.llm = ChatOpenAI(model_name=OPENAI_API_MODEL)

            # Criar pipeline de perguntas e respostas
            self.qa_chain = ConversationalRetrievalChain.from_llm(
                self.llm, self.vector_store.as_retriever(), memory=self.memory,
                combine_docs_chain_kwargs={"prompt": self.get_custom_prompt()}
            )
        except Exception as e:
            print(f"Error on loading embeddings: {e}")
            self.vector_store = None

    def get_custom_prompt(self) -> PromptTemplate:
        return PromptTemplate(
            input_variables=["context", "question"],
            template="""
            Você é um chatbot que responde como **Fabricio Souza**, um engenheiro de software experiente. 
            Seu objetivo é responder perguntas sobre Fabricio e também sobre tecnologia, programação e engenharia de software, sempre mantendo a conversa dentro do contexto apropriado.

            - Sempre responda em primeira pessoa, como se fosse Fabricio.
            - Seja simpático e objetivo.
            - Se houver Contexto relevante, use-o para fundamentar sua resposta.
            - Se a pergunta for técnica, responda como um engenheiro de software experiente, sem fugir do Contexto. Exemplo: Se Fabricio são sabe Kotlin, não pode opinar muito sobre esse assunto.
            - Se não souber a resposta ou a pergunta for irrelevante, responda educadamente.

            ---

            **Contexto:**
            {context}

            **Pergunta**  
            {question}
            """
        )

    def get_response(self, question: str) -> str:
        """Receive a question nd returns the response based on embeddings."""
        if not self.vector_store:
            return "❌ Erro: Knowledge base not loaded."

        try:
            response = self.qa_chain({"question": question})
            return response["answer"]
        except Exception as e:
            return f"Erro on processing the response: {str(e)}"

