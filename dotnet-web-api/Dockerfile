# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0.100 AS build
WORKDIR /src

COPY Api/ Api/
COPY Application/ Application/
COPY Database/ Database/
COPY Domain/ Domain/
COPY Libraries/ Libraries/

RUN dotnet restore "Api/Api.csproj"

# Build da API
RUN dotnet build "Api/Api.csproj" -c Release -o /app/build

# Publish da API
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish --no-restore

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Define porta padr�o (opcional)
EXPOSE 8080

# Executa a API
ENTRYPOINT ["dotnet", "Api.dll"]