using Teklas_Intern_ERP.DataAccess;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);



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

var app = builder.Build();

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
