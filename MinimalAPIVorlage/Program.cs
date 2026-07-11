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
string binPath = AppContext.BaseDirectory;
string dbPath = Path.Combine(binPath, "MeinDB.db");

builder.Services.AddDatabase($"Data Source={dbPath}");


// 🔹 Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IProductService, ProductService>();

// Endpoint-Registrierung
builder.Services.AddScoped<IEndpointDefinition, ProductEndpoints>();

// 🔹 CORS - Vorlage erlaubt aktuell ein lokales Dev-Frontend.
// Vor Produktivbetrieb unbedingt an die echten Frontend-Origins anpassen
// (siehe README, Abschnitt "Sicherheitshinweis").
const string CorsPolicyName = "DefaultCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
        policy.WithOrigins("https://localhost:3000", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// 🔹 Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Standard: /swagger
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

// ALLE Endpoints hier
app.RegisterEndpoints();

app.Run();