# Task Manager

Full-stack task management web application built with **ASP.NET Core** and **Blazor**, following **Clean Architecture** principles.

## 🚀 Features
- User registration and login (JWT authentication)
- Task management (create, update, delete)
- Secure REST API
- Separation of concerns using Clean Architecture
- PostgreSQL database with Entity Framework Core

## 🧱 Tech Stack

### Backend
- ASP.NET Core
- Clean Architecture
- Entity Framework Core
- PostgreSQL
- JWT Authentication

### Frontend
- Blazor
- HTTP API integration
- JWT-based authorization

### Other
- Git
- Docker (optional)
- Swagger / OpenAPI

## 📁 Project Structure

```text
TaskManager/
├── Backend/
│   ├── Api/
│   │   ├── Controllers/           # API controllers (Auth, Tasks, etc.)
│   │   ├── Program.cs             # Application entry point
│   │   ├── appsettings.json       # Base configuration (no secrets)
│   │   ├── appsettings.Development.json # Local dev config (ignored by Git)
│   │   └── Api.csproj
│   │
│   ├── Core/
│   │   ├── Domain/                # Domain entities
│   │   ├── Application/
│   │   │   ├── DTOs/              # Data Transfer Objects
│   │   │   ├── Interfaces/        # Service & repository interfaces
│   │   │   └── Services/          # Business logic
│   │   └── Core.csproj
│   │
│   ├── Infrastructure/
│   │   ├── Data/                  # DbContext, migrations
│   │   ├── Repositories/          # EF Core repositories
│   │   ├── Auth/                  # JWT / security implementations
│   │   └── Infrastructure.csproj
│
├── Frontend/
│   └── TaskManager_Client/
│       ├── Pages/                 # Blazor pages
│       ├── Components/            # Reusable UI components
│       ├── Services/              # HTTP API services
│       ├── Program.cs             # Frontend entry point
│       └── TaskManager_Client.csproj
│
├── Tests/
│   ├── Unit/                      # Unit tests
│   ├── Integration/               # Integration tests
│   └── Tests.csproj
│
├── .gitignore
├── README.md
└── TaskManager.sln

📌 Author

Created by Oleg Pona Junior Backend Developer (.NET)
