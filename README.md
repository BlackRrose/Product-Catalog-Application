# 🛍️ Product Catalog Web Application

## 📌 Objective
A lightweight product catalog web application built with **.NET Core** and **C#**.  
The solution demonstrates end-to-end development with clean architecture, SOLID principles, design patterns, caching, unit testing, and a responsive UI.

The app integrates with the [FakeStore API](https://fakestoreapi.com/) to fetch product data, exposes its own REST API, and serves a responsive front-end for browsing products.

---

## 🚀 Features

### ✅ External API Consumption
- Consumes **FakeStore API** for product data.
- Implements a **typed API client service** with dependency injection.
- Handles asynchronous calls and error handling.

### ✅ Custom REST API
- Exposes `/api/products` endpoint.
- Supports:
  - Filtering by category
  - Pagination
  - Sorting
- Caching implemented to boost performance.

### ✅ SOLID & Design Patterns
- Service, controller, and data layers follow **SOLID principles**.
- Clean separation of concerns via abstractions.
- Applied patterns: **Repository Pattern**, **Decorator Pattern** (for caching).

### ✅ Unit Testing
- Tests for Product Service and API Client.
- Uses **nUnit** and **Moq** for mocking dependencies.

### ✅ Web UI (ASP.NET Core MVC or Blazor)
- Product listing with filtering, searching, and detail view.
- **Responsive** design using Bootstrap.
- AJAX-based product filtering/live search with JavaScript/jQuery.
- UI consumes **our own API** (not FakeStore directly).

### ✅ Bonus Features
- Optional **Umbraco** integration as front-end.
- Example CI/CD pipeline using `.yaml`.

---

## 🏗️ Architecture Overview

The solution is structured into clean layers:

- **API Client Layer** → Fetches and transforms data from FakeStore API.
- **Application/Service Layer** → Business logic, caching, filtering, pagination, sorting.
- **API Layer** → Exposes REST API (`/api/products`).
- **UI Layer** → ASP.NET Core MVC/Blazor front-end.
- **Tests** → Unit tests with nUnit & Moq.

### 🔑 SOLID Application
- **S**ingle Responsibility → Each class/service has one responsibility.
- **O**pen/Closed → Extend behavior without modifying existing code.
- **L**iskov Substitution → Abstractions instead of concrete implementations.
- **I**nterface Segregation → Small, focused interfaces.
- **D**ependency Inversion → High-level modules depend on abstractions.

### ⚡ Caching Strategy
- In-memory caching for product lists.
- Expiration-based invalidation.
- Implemented with **Decorator Pattern** for clean separation.


---

## 🛠️ Getting Started

### Prerequisites
- .NET 6+ SDK  
- Visual Studio 2022 / Rider / VS Code  
- SQL Server LocalDB (only if persistence layer is added)

### Running Locally
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/product-catalog.git
   cd product-catalog
   dotnet restore
   dotnet build
   dotnet run --project src/ProductCatalog.Web

Access in your browser:
UI → http://localhost:5000
API → http://localhost:5000/api/products

