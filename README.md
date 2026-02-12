# Task Manager API

Production-oriented task management system built with **ASP.NET Core**, following **Clean Architecture** principles.
The project focuses on backend architecture, authentication, and clean separation of responsibilities.

> This project was created to demonstrate real-world backend development practices for a Junior .NET Developer role.

---

## 🚀 Key Features

### 🔐 Authentication & Authorization
- JWT-based authentication (Access + Refresh tokens)
- Secure login / logout flow
- Role-based authorization (`User`, `Admin`)
- Token validation via middleware

### 📋 Task Management
- Full CRUD operations for tasks
- User-scoped data access
- Validation at application layer
- RESTful API design with proper HTTP status codes

### 🧱 Architecture
- Clean Architecture (Domain → Application → Infrastructure → API)
- Business logic isolated from infrastructure concerns
- Dependency Injection across all layers
- Easily testable services

---

## 🏗️ Architecture Overview

The project follows **Clean Architecture** to ensure:
- High testability
- Loose coupling
- Clear separation of concerns

### Layers:
- **Core** — Domain models, business rules, interfaces
- **Infrastructure** — EF Core, database access, authentication implementation
- **Api** — ASP.NET Core Web API, controllers, middleware
- **Frontend** — Blazor client consuming the REST API
- **Tests** — Unit and integration tests for business logic

---

## 🧪 Testing
- Unit tests for application services
- Business logic tested independently from controllers
- Authentication and validation scenarios covered

---

## 🗄️ Database
- PostgreSQL
- Entity Framework Core
- Code-first migrations
- Async database operations

---

## 🧰 Tech Stack

### Backend
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Clean Architecture

### Frontend
- Blazor
- REST API integration
- JWT-based authorization

### Tooling
- Git
- Swagger / OpenAPI
- Docker (optional)

---

## ▶️ Running the Project

1. Clone the repository
2. Configure connection string and JWT settings
3. Run database migrations
4. Start the API and Blazor client

Swagger UI is available for API testing.

---

## 📌 Purpose of the Project

This project was built to:
- Practice real-world backend architecture
- Implement secure authentication
- Demonstrate readiness for a **Junior .NET Backend Developer** position

---

## 👤 Author

**Oleg Pona**  
Junior Backend Developer (.NET)  
