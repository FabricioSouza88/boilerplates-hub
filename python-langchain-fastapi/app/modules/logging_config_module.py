import os
import logging
import time
from fastapi import Request
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.responses import Response
import json

def configure_logging(app):
    app.add_middleware(AuditLogMiddleware)

LOG_FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "audit.log")

# Configuração do logger para salvar no arquivo audit.log
logging.basicConfig(
    filename=LOG_FILE,  # Nome do arquivo de log
    level=logging.INFO,  # Nível do log
    format="%(asctime)s - %(levelname)s - %(message)s",  # Formato do log
)

class AuditLogMiddleware(BaseHTTPMiddleware):
    """Middleware to log audit of requests and responses"""

    async def dispatch(self, request: Request, call_next):
        start_time = time.time()  # Marca o tempo inicial

        # Obtendo o corpo da request (caso tenha um payload)
        body = await request.body()
        request_data = body.decode("utf-8") if body else None

        # Processando a resposta
        response = await call_next(request)
        process_time = time.time() - start_time  # Calcula o tempo de resposta

        # Tentando capturar a resposta em JSON, se aplicável
        response_body = b""
        async for chunk in response.body_iterator:
            response_body += chunk

        response_content = response_body.decode("utf-8")
        try:
            response_json = json.loads(response_content)
        except json.JSONDecodeError:
            response_json = response_content  # Caso a resposta não seja JSON, salva como string

        # Criando log com request e response
        log_data = {
            "ip": request.client.host,
            "method": request.method,
            "url": request.url.path,
            "request_body": request_data,
            "status_code": response.status_code,
            "response_body": response_json,
            "time_taken": f"{process_time:.4f}s",
        }

        logging.info(f"AUDIT LOG: {json.dumps(log_data, ensure_ascii=False)}")  # Salva no log

        # Retornando a resposta original
        return Response(content=response_body, status_code=response.status_code, headers=dict(response.headers), media_type=response.media_type)