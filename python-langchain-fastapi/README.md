## Generate/Update Embeddings

### With Python
```sh
python app/services/embeddings_service.py
```
### With Docker
```sh
docker run --rm --env-file .env -v $(pwd)/data:/app/data -v $(pwd)/embeddings:/app/embeddings chatbot-api python app/services/embeddings_service.py
```

## Start API
### With Python
```sh
uvicorn app.main:app --host 0.0.0.0 --port 8002
```
### With Docker
```sh
docker-compose up --build
```

## Test API
```sh
curl -X POST "http://localhost:8002/chat" -H "Content-Type: application/json" -d '{"question": "Quais as 3 linguagens de programação você possui maior experiência?"}'
```
