# 🚀 .NET Web API Starter

This is a pre-configured **ASP.NET Core 8 Web API starter** project, designed to provide a solid and modern foundation for building scalable, secure, and testable APIs using C#.

It is part of the [boilerplates-hub](../README.md) repository.

---

## ✅ Features

- ASP.NET Core 8 (LTS)
- JWT Authentication
- Role-based Authorization
- Global Error Handling Middleware
- Swagger/OpenAPI with JWT support
- Clean Architecture structure (Api, Application, Domain, Infrastructure)
- Dependency Injection (built-in)
- Dockerfile + Docker Compose
- Logging (ILogger ready, extendable to Serilog)
- Unit Testing with xUnit

---

## 🧱 Project Structure

dotnet-web-api/
├── Api/ → ASP.NET Core entrypoint (controllers, Program.cs)
├── Application/ → Business use cases and application logic
├── Domain/ → Entities, enums, interfaces, core domain logic
├── Infrastructure/ → Repositories, database, service integrations
├── Tests/ → Unit and integration tests (xUnit)
├── Dockerfile
├── docker-compose.yml
└── README.md

## 👥 Contributing
Contributions are welcome! If you'd like to add a new starter or improve existing ones, feel free to open an issue or submit a pull request.

## 📄 License
This project is licensed under the MIT License.

## 💡 Inspiration
This repository was created to consolidate proven patterns and practices into reusable starters that enable faster and cleaner API development from day one.