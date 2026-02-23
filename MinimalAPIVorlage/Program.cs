using DataHandler;
using DataModels.Entities;
using DBUmgebung;
using DBUmgebung.Repositories;
using Microsoft.EntityFrameworkCore;
using ProduktService;
using ProduktService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// SQLite
string binPath = AppContext.BaseDirectory;
string dbPath = Path.Combine(binPath, "MeinDB.db");

builder.Services.AddDatabase($"Data Source={dbPath}");


// 🔹 Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericDataHandler<>), typeof(GenericDataHandler<>));

builder.Services.AddScoped<IProductDataHandler, ProductDataHandler>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// 🔹 Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Standard: /swagger
}

app.MapGet("/products", async (IProductService service) =>
    await service.GetAllAsync());

app.MapPost("/products", async (
    IProductService service,
    Common.ApiRequest<Product> request) =>
        await service.CreateAsync(request));

app.Run();