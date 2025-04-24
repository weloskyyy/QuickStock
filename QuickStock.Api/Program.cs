using Microsoft.EntityFrameworkCore;
using QuickStock.Infrastructure.Data;
using QuickStock.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7059")  // URL de tu aplicación web
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// REGISTRO DE SERVICIOS
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTRAR DbContext
builder.Services.AddDbContext<QuickStockDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// REGISTRAR REPOSITORIOS
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<SaleRepository>();


var app = builder.Build();

// MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS - IMPORTANTE: debe ir antes de UseAuthorization y MapControllers
app.UseCors("AllowWebApp");

app.UseAuthorization();

app.MapControllers();

app.Run();