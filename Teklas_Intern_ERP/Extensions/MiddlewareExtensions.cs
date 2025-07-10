using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace Teklas_Intern_ERP.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var error = exceptionHandlerPathFeature?.Error;
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(error, "Unhandled exception occurred");
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Beklenmeyen bir hata oluştu." }));
                });
            });
            return app;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/MaterialManagement/swagger.json", "Material Management");
                    c.SwaggerEndpoint("/swagger/UserManagement/swagger.json", "User Management");
                    c.SwaggerEndpoint("/swagger/ProductionManagement/swagger.json", "Production Management");
                    c.SwaggerEndpoint("/swagger/WarehouseManagement/swagger.json", "Warehouse Management");
                    c.SwaggerEndpoint("/swagger/PurchasingManagement/swagger.json", "Purchasing Management");
                    c.SwaggerEndpoint("/swagger/SalesOrderManagement/swagger.json", "Sales & Order Management");
                });
            }
            return app;
        }
    }
} 