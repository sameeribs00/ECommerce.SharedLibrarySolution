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

### Architectural Benefits

- **Modularity**: Each component is self-contained and reusable
- **Testability**: Interface-based design enables easy mocking
- **Scalability**: Generic patterns support multiple entity types
- **Maintainability**: Centralized configuration and error handling
- **Security**: Built-in JWT authentication and API gateway protection

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

### Integration in Your Project

1. **Add Package Reference**
   ```xml
   <PackageReference Include="ECommerece.CommonLibrary" Version="1.0.0" />
   ```

2. **Register Services in Program.cs**
   ```csharp
   using ECommerece.CommonLibrary.DependencyInjection;
   
   var builder = WebApplication.CreateBuilder(args);
   
   // Register shared services
   builder.Services.AddSharedService<YourDbContext>(builder.Configuration, "YourAppName");
   
   var app = builder.Build();
   
   // Add shared middlewares
   app.AddSharedMiddlewares();
   
   app.Run();
   ```

## 💡 Usage Examples

### Generic Service Usage

```csharp
public class ProductService : IGenericService<Product>
{
    private readonly IGenericService<Product> _genericService;
    
    public ProductService(IGenericService<Product> genericService)
    {
        _genericService = genericService;
    }
    
    public async Task<BaseResponse> GetProductsByCategory(int categoryId)
    {
        return await _genericService.GetByAsync(p => p.CategoryId == categoryId);
    }
}
```

### Excel Export Usage

```csharp
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IExcelExportService _excelService;
    
    public ReportsController(IExcelExportService excelService)
    {
        _excelService = excelService;
    }
    
    [HttpGet("export-products")]
    public async Task<IActionResult> ExportProducts()
    {
        var products = await GetProductsAsync();
        var excelData = await _excelService.ExportToExcel(products, "Products", "ProductReport");
        
        return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "products.xlsx");
    }
}
```

### JWT Authentication Usage

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Uses JWT authentication from shared library
public class SecureController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecureData()
    {
        return Ok(new { Message = "This is secure data", User = User.Identity.Name });
    }
}
```

### Custom Headers Excel Export

```csharp
public async Task<byte[]> ExportWithCustomHeaders()
{
    var data = await GetReportDataAsync();
    var customHeaders = new Dictionary<string, string>
    {
        { "Id", "Product ID" },
        { "Name", "Product Name" },
        { "Price", "Unit Price" },
        { "Stock", "Available Stock" }
    };
    
    return await _excelService.ExportToExcelWithCustomHeaders(data, customHeaders, "Product Report");
}
```

## 🔧 Configuration Options

### Logging Configuration

The library automatically configures Serilog with:
- **Console Output**: For development debugging
- **File Output**: Daily rolling files with timestamps
- **Debug Output**: For Visual Studio debugging
- **Structured Logging**: JSON format for production

### Database Configuration

- **Connection Retry**: Automatic retry on connection failures
- **Multiple Active Result Sets**: Enabled for complex queries
- **Generic DbContext Support**: Works with any EF Core DbContext

### JWT Configuration

- **Token Validation**: Issuer, audience, and signing key validation
- **Lifetime Validation**: Configurable (currently disabled for development)
- **HTTPS Requirement**: Configurable for different environments

## 🧪 Testing

### Unit Testing

```csharp
[Test]
public async Task GenericService_GetById_ReturnsCorrectData()
{
    // Arrange
    var mockService = new Mock<IGenericService<Product>>();
    var expectedResponse = new BaseResponse(true, "Success", new Product { Id = 1 });
    mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(expectedResponse);
    
    // Act
    var result = await mockService.Object.GetByIdAsync(1);
    
    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
}
```

### Integration Testing

```csharp
[Test]
public async Task ExcelExport_GeneratesValidFile()
{
    // Arrange
    var testData = new List<Product> { new Product { Id = 1, Name = "Test" } };
    var excelService = new ExcelExportService();
    
    // Act
    var result = await excelService.ExportToExcel(testData);
    
    // Assert
    Assert.NotNull(result);
    Assert.True(result.Length > 0);
}
```

## 📝 API Documentation

### BaseResponse Structure

```csharp
public record BaseResponse(
    bool IsSuccess = false, 
    string Message = null!, 
    dynamic Data = null!
);
```

### IGenericService Interface

```csharp
public interface IGenericService<T> where T : class
{
    Task<BaseResponse> CreateAsync(T entity);
    Task<BaseResponse> UpdateAsync(T entity);
    Task<BaseResponse> DeleteAsync(T entity);
    Task<BaseResponse> GetAllAsync();
    Task<BaseResponse> GetByIdAsync(int id);
    Task<BaseResponse> GetByAsync(Expression<Func<T, bool>> predicate);
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation wiki

## 🔄 Version History

- **v1.0.0** - Initial release with core functionality
  - JWT Authentication
  - Generic Service Pattern
  - Excel Export Service
  - Global Exception Handling
  - API Gateway Middleware

---

**Built with ❤️ for the ECommerce ecosystem**
