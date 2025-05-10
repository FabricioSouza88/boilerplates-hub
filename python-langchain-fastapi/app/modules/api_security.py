from fastapi import Header, HTTPException
from app.modules.config import TTM_API_KEY

async def validate_api_key(ttm_api_key: str = Header(None)):
    """Valida a API Key recebida no cabeçalho."""
    if ttm_api_key is None or ttm_api_key != TTM_API_KEY:
        raise HTTPException(status_code=403, detail="Access Denied: Invalid API Key")
    return True
