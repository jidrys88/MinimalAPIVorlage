using DataModels.Entities;
using DBUmgebung;
using DBUmgebung.Repositories;
using Microsoft.EntityFrameworkCore;
using MinimalAPIVorlage.EndPoints;
using ProduktService;
using ProduktService.Interfaces;

// Test Github

var builder = WebApplication.CreateBuilder(args);

// SQLite
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionString 'Default' fehlt in appsettings.json");

builder.Services.AddDatabase(connectionString);


// 🔹 Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IProductService, ProductService>();

// Endpoint-Registrierung
builder.Services.AddScoped<IEndpointDefinition, ProductEndpoints>();



var app = builder.Build();

// Ausstehende Migrationen automatisch anwenden
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// 🔹 Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Standard: /swagger
}

app.UseHttpsRedirection();

// ALLE Endpoints hier
app.RegisterEndpoints();

app.Run();