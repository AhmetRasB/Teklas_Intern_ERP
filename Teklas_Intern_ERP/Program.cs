using Teklas_Intern_ERP.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin() // Geliştirme için, prod'da domain belirtin!
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseCors(); // app.UseAuthorization()'dan önce olmalı!

app.UseAuthorization();

app.MapControllers();

app.Run();
