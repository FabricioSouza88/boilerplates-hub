from slowapi import Limiter, _rate_limit_exceeded_handler
from slowapi.util import get_remote_address
from slowapi.middleware import SlowAPIMiddleware
from slowapi.errors import RateLimitExceeded

# Criando o Rate Limiter baseado no IP do usuário
limiter = Limiter(key_func=get_remote_address)

def configure_rate_limiting(app):
    """Configure Rate Limiter for FastAPI App."""
    # Middleware para Rate Limiting
    app.state.limiter = limiter
    app.add_middleware(SlowAPIMiddleware)

    # Global handler for requests that hit the rate limit
    app.add_exception_handler(RateLimitExceeded, _rate_limit_exceeded_handler)
