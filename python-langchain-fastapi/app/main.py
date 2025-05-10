from openai import BaseModel
from fastapi import Depends, HTTPException, Request
from app.services.chat_service import ChatService
from app.modules import fastapi_module, api_security
from app.modules.rate_limiting_config_module import limiter

# Initialize FastAPI and ChatService
app = fastapi_module.create_app()
chat_service = ChatService()

class QuestionRequest(BaseModel):
    question: str

@app.get("/hello")
async def hello():
    return { 'response': "Hi. What's Up!" }

@app.post("/chat", dependencies=[Depends(api_security.validate_api_key)])
@limiter.limit("10/minute")  # Máx. 10 requests per minute
async def chat(request: Request, chat_request: QuestionRequest):
    """Receive a question and returns a response based on embeddings."""
    try:
        response = chat_service.get_response(chat_request.question)
        return {"response": response}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
