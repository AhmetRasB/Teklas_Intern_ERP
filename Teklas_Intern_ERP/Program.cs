using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Teklas_Intern_ERP.DataAccess;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Business.MaterialManagement;
using Teklas_Intern_ERP.Business.Mapping;
using Teklas_Intern_ERP.Extensions;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.DataAccess.MaterialManagement;

var builder = WebApplication.CreateBuilder(args);

// Serilog konfigürasyonunu builder'a ekle
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// FluentValidation yeni nesil registration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<MaterialCardDto>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("MaterialManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Material Management", Version = "v1" });
    //c.SwaggerDoc("ProductionManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Production Management", Version = "v1" });
    //c.SwaggerDoc("WarehouseManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Warehouse Management", Version = "v1" });
    //c.SwaggerDoc("PurchasingManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Purchasing Management", Version = "v1" });
    //c.SwaggerDoc("SalesOrderManagement", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Sales & Order Management", Version = "v1" });
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
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
// Register repository interfaces
builder.Services.AddScoped<IMaterialCardRepository, MaterialCardRepository>();
builder.Services.AddScoped<IMaterialCategoryRepository, MaterialCategoryRepository>();

// Register services with proper constructors
builder.Services.AddScoped<IMaterialCardService, MaterialCardService>();
builder.Services.AddScoped<IMaterialCategoryService, MaterialCategoryService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "TeklasERP_";
});

var app = builder.Build();

// Global exception handler
app.UseCustomExceptionHandler();

app.UseSerilogRequestLogging();

// Swagger configuration
app.UseCustomSwagger(app.Environment);

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
