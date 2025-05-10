import os
from dotenv import load_dotenv

# Carrega variáveis do arquivo .env
load_dotenv()

ENV = os.getenv("ENV")
TTM_API_KEY = os.getenv("TTM_API_KEY")
ALLOWED_ORIGINS = os.getenv("ALLOWED_ORIGINS").split(",")
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY")
OPENAI_API_MODEL = os.getenv("OPENAI_API_MODEL")