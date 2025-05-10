from fastapi.middleware.cors import CORSMiddleware

from app.modules.config import ENV, ALLOWED_ORIGINS

def configure_cors(app):
    """Configure o CORS for the FastAPI App."""
    # Configure CORS
    allowed_origins = ["*"] if is_running_locally() else ALLOWED_ORIGINS
    print(f"Allowed origins: {allowed_origins}")
    app.add_middleware(
        CORSMiddleware,
        allow_origins=allowed_origins,
        allow_credentials=True,
        allow_methods=["POST", "GET", "OPTIONS", "HEAD"],
        allow_headers=["*"],
    )

def is_running_locally():
    return ENV != None and ENV == "localhost"