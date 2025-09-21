using ECommerece.CommonLibrary.Helpers.ExcelHelper;
using ECommerece.CommonLibrary.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace ECommerece.CommonLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TDbContext> (this IServiceCollection serviceCollection, IConfiguration config, string fileName ) where TDbContext : DbContext
        {
            //Add Generic DB Context
            serviceCollection
                .AddDbContext<TDbContext>(option => option.UseSqlServer(config.GetConnectionString("eCommerceConnection"), 
                                          sqlServerOption => sqlServerOption.EnableRetryOnFailure()));

            //Configure Serilog Logging  
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                               path: $"{fileName}-.text",
                               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                               rollingInterval: RollingInterval.Day                                
                              )
                .CreateLogger();

            //Inject the export to excel service
            serviceCollection.AddScoped<IExcelExportService, ExcelExportService>();

            //Add JWT Configuration 
            //serviceCollection.AddJwtAuthenticationScheme(config); => can be used as an Extension..
            JWTAuthenticationScheme.AddJwtAuthenticationScheme(serviceCollection, config);

            return serviceCollection;
        }

        public static IApplicationBuilder AddSharedMiddlewares (IApplicationBuilder app)
        {
            //Add Exception Handler Middleware
            app.UseMiddleware<GlobalExceptionMiddleware>();
            //Add Gateway Requests Gaurd
            //app.UseMiddleware<AllowOnlyApiGatewayMiddleware>();

            return app;
        }
    }
}
