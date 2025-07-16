using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.Extensions;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.Business.MaterialManagement;
using Teklas_Intern_ERP.Business.UserManagement;
using Teklas_Intern_ERP.Business.WarehouseManagement;
using Teklas_Intern_ERP.Business.PurchasingManagement;
using Teklas_Intern_ERP.Business.SalesManagement;
using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.UserManagement;
using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess.SalesManagement;
using Teklas_Intern_ERP.Business.Mapping;
using Teklas_Intern_ERP.Business.Helpers;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.ProductionManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("MaterialManagement", new OpenApiInfo { Title = "Material Management", Version = "v1" });
    c.SwaggerDoc("UserManagement", new OpenApiInfo { Title = "User Management", Version = "v1" });
    c.SwaggerDoc("ProductionManagement", new OpenApiInfo { Title = "Production Management", Version = "v1" });
    c.SwaggerDoc("WarehouseManagement", new OpenApiInfo { Title = "Warehouse Management", Version = "v1" });
    c.SwaggerDoc("PurchasingManagement", new OpenApiInfo { Title = "Purchasing Management", Version = "v1" });
    c.SwaggerDoc("SalesManagement", new OpenApiInfo { Title = "Sales Management", Version = "v1" });
    
    // JWT Bearer Authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.GroupName ?? string.Empty;
        return docName == groupName.Replace(" ", "");
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(EntityMappingProfile));

// Add Repositories - Monolithic Architecture
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMaterialCardRepository, MaterialCardRepository>();
builder.Services.AddScoped<IMaterialCategoryRepository, MaterialCategoryRepository>();
builder.Services.AddScoped<IMaterialMovementRepository, MaterialMovementRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IStockEntryRepository, StockEntryRepository>();
builder.Services.AddScoped<ISupplierTypeRepository, SupplierTypeRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerOrderRepository, CustomerOrderRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IProductionConfirmationRepository, ProductionConfirmationRepository>();

// Add Services - Interface to Implementation mappings for proper DI
builder.Services.AddScoped<IMaterialCardService, MaterialCardService>();
builder.Services.AddScoped<IMaterialCategoryService, MaterialCategoryService>();
builder.Services.AddScoped<IMaterialMovementService, MaterialMovementService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IStockEntryService, StockEntryService>();
builder.Services.AddScoped<ISupplierTypeService, SupplierTypeService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerOrderService, CustomerOrderService>();
builder.Services.AddScoped<IBillOfMaterialService, BillOfMaterialService>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IProductionConfirmationService, ProductionConfirmationService>();

// Add JWT Helper
builder.Services.AddScoped<JwtHelper>();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new ArgumentException("JWT SecretKey is missing in configuration");
}

var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/MaterialManagement/swagger.json", "Material Management");
        c.SwaggerEndpoint("/swagger/UserManagement/swagger.json", "User Management");
        c.SwaggerEndpoint("/swagger/ProductionManagement/swagger.json", "Production Management");
        c.SwaggerEndpoint("/swagger/WarehouseManagement/swagger.json", "Warehouse Management");
        c.SwaggerEndpoint("/swagger/PurchasingManagement/swagger.json", "Purchasing Management");
        c.SwaggerEndpoint("/swagger/SalesManagement/swagger.json", "Sales Management");
    });
    
    // Global Exception Handler for Development - Show detailed errors
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            
            var response = new
            {
                error = "Internal Server Error",
                message = exception?.Message ?? "An error occurred",
                details = exception?.ToString(), // Full exception details in development
                stackTrace = exception?.StackTrace
            };
            
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        });
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
