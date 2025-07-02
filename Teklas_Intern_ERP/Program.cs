using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.Mapping;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// Serilog konfigürasyonunu builder'a ekle
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
);
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<Program>();
        fv.AutomaticValidationEnabled = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("MaterialManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Material Management", Version = "v1" });
    c.SwaggerDoc("ProductionManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Production Management", Version = "v1" });
    c.SwaggerDoc("WarehouseManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Warehouse Management", Version = "v1" });
    c.SwaggerDoc("PurchasingManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Purchasing Management", Version = "v1" });
    c.SwaggerDoc("SalesOrderManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Sales & Order Management", Version = "v1" });
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.GroupName ?? string.Empty;
        return docName == groupName.Replace(" ", "");
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin() 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(typeof(EntityMappingProfile).Assembly);
builder.Services.AddScoped<Teklas_Intern_ERP.DataAccess.MaterialManagement.MaterialCardRepository>();
builder.Services.AddScoped<Teklas_Intern_ERP.Business.MaterialManagement.MaterialCardManager>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "TeklasERP_";
});

var app = builder.Build();

// Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var error = exceptionHandlerPathFeature?.Error;
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(error, "Unhandled exception occurred");
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { error = "Beklenmeyen bir hata oluştu." }));
    });
});

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/MaterialManagement/swagger.json", "Material Management");
        c.SwaggerEndpoint("/swagger/ProductionManagement/swagger.json", "Production Management");
        c.SwaggerEndpoint("/swagger/WarehouseManagement/swagger.json", "Warehouse Management");
        c.SwaggerEndpoint("/swagger/PurchasingManagement/swagger.json", "Purchasing Management");
        c.SwaggerEndpoint("/swagger/SalesOrderManagement/swagger.json", "Sales & Order Management");
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
