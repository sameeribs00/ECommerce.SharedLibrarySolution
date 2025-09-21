# ECommerce.SharedLibrarySolution

A comprehensive .NET 8 shared library solution designed to provide common functionality, services, and infrastructure components for e-commerce applications. This library encapsulates reusable patterns, authentication mechanisms, data access utilities, and cross-cutting concerns to accelerate development across multiple e-commerce microservices.

## 🎯 Purpose

This shared library serves as a foundational layer for e-commerce applications, providing:

- **Standardized Authentication & Authorization** with JWT Bearer tokens
- **Generic Data Access Patterns** for consistent CRUD operations
- **Excel Export Functionality** for data reporting and analytics
- **Global Exception Handling** with structured error responses
- **API Gateway Integration** for microservice architecture
- **Comprehensive Logging** with Serilog integration
- **Dependency Injection** container configuration

## 🛠️ Technical Stack

### Core Technologies
- **.NET 8.0** - Latest LTS version with modern C# features
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 8.0.7** - ORM for data access
- **SQL Server** - Primary database provider

### Key Dependencies
- **Microsoft.AspNetCore.Authentication.JwtBearer (8.0.7)** - JWT authentication
- **Microsoft.EntityFrameworkCore.SqlServer (8.0.7)** - SQL Server provider
- **Serilog.AspNetCore (8.0.1)** - Structured logging
- **ClosedXML (0.105.0)** - Excel file generation

### Development Tools
- **Visual Studio 2022** - Primary IDE
- **C# 12** - Latest language features with nullable reference types

## 🏗️ Architecture & Design Patterns

### Design Patterns Implemented

1. **Dependency Injection Pattern**
   - Centralized service registration through `SharedServiceContainer`
   - Generic service container supporting any `DbContext`
   - Extension methods for clean API integration

2. **Generic Repository Pattern**
   - `IGenericService<T>` interface for type-safe CRUD operations
   - Expression-based filtering with `Expression<Func<T, bool>>`
   - Consistent response structure with `BaseResponse`

3. **Middleware Pattern**
   - `GlobalExceptionMiddleware` for centralized error handling
   - `AllowOnlyApiGatewayMiddleware` for API gateway protection
   - Pipeline-based request/response processing

4. **Service Layer Pattern**
   - `IExcelExportService` for data export functionality
   - Separation of concerns with dedicated service interfaces
   - Async/await pattern throughout

5. **Configuration Pattern**
   - JWT authentication configuration through `JWTAuthenticationScheme`
   - Environment-based configuration management
   - Connection string management with retry policies


## 📦 Project Structure

```
ECommerece.CommonLibrary/
├── DependencyInjection/          # DI container configuration
│   ├── SharedServiceContainer.cs # Main service registration
│   └── JWTAuthenticationScheme.cs # JWT configuration
├── Generics/                     # Generic service patterns
│   └── IGenericService.cs       # Generic CRUD interface
├── Helpers/                      # Utility services
│   ├── ExcelHelper/             # Excel export functionality
│   │   ├── IExcelExportService.cs
│   │   └── ExcelExportService.cs
│   └── GlobalUsings.cs          # Global using statements
├── Logs/                        # Logging utilities
│   └── LogException.cs          # Exception logging
├── Middlewares/                 # HTTP pipeline components
│   ├── GlobalExceptionMiddleware.cs
│   └── AllowOnlyApiGatewayMiddleware.cs
└── Responses/                   # Response models
    └── BaseResponse.cs          # Standardized response structure
```

## 🚀 Setup & Installation

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full instance)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd ECommerce.SharedLibrarySolution
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the Solution**
   ```bash
   dotnet build
   ```

4. **Configure Connection String**
   
   Add to your `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "eCommerceConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

5. **Configure JWT Authentication**
   
   Add to your `appsettings.json`:
   ```json
   {
     "Authentication": {
       "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
       "Issuer": "ECommerce.API",
       "Auddience": "ECommerce.Client"
     }
   }
   ```

