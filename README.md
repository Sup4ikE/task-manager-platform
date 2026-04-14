# ⚡ Task Manager Platform

Modern task management platform built with ASP.NET Core using Clean Architecture, focused on scalable backend design and secure authentication.

---

## 🚀 Tech Stack

### 🧠 Backend

* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* Clean Architecture

### 🎨 Frontend

* Blazor
* REST API integration

### ⚙️ Tooling

* Swagger / OpenAPI
* Git
* Docker (optional)

---

## ✨ Features

* 🔐 JWT Authentication (Access + Refresh tokens)
* 🛡️ Role-based access control (User / Admin)
* 📋 Full CRUD for task management
* 👤 User-scoped data isolation
* ✅ Validation at application layer
* 🔁 Middleware-based token validation
* 📘 Swagger UI for API exploration
* 🌐 RESTful API design

---

## 🏗️ Architecture

```
Domain → Application → Infrastructure → API
```

* Clear separation of concerns
* Business logic isolated from infrastructure
* Dependency Injection across all layers
* Designed for scalability and testability

---

## 📂 Project Structure

```
task-manager-platform/
│
├── TaskManager.Api
├── TaskManager.Core
├── TaskManager.Infrastructure
├── TaskManager.Tests
├── frontend (Blazor)
```

---

## 🧪 Testing

* Unit tests for application services
* Business logic tested independently
* Authentication and validation covered

---

## 🗄️ Database

* PostgreSQL
* Entity Framework Core (Code First)
* Async queries & migrations

---

## ▶️ Running the Project

1. Clone repository
2. Configure `appsettings.json` (DB + JWT)
3. Run migrations
4. Start API and frontend

Swagger available for API testing.

---

## 👨‍💻 Author

**Oleg Pona**

---

## ⭐ About

This project demonstrates real-world backend development practices, including clean architecture, authentication, and scalable API design.


