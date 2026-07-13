using System.Text;
using DBUmgebung;
using DBUmgebung.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIVorlage;
using MinimalAPIVorlage.EndPoints;
using ProduktService;
using ProduktService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// SQLite - Connection-String kommt ausschließlich aus appsettings.json (einzige Quelle der Wahrheit)
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionString 'Default' fehlt in appsettings.json");

builder.Services.AddDatabase(connectionString);

// 🔹 Swagger Services (inkl. "Authorize"-Button für Bearer-Token)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Bearer Token eingeben (ohne 'Bearer '-Prefix)"
    });

    // Schloss-Symbol nur bei Endpoints mit .RequireAuthorization() anzeigen,
    // nicht global bei jedem Endpoint (z. B. /auth/login soll offen bleiben).
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Endpoint-Registrierung
builder.Services.AddScoped<IEndpointDefinition, ProductEndpoints>();
builder.Services.AddScoped<IEndpointDefinition, AuthEndpoints>();

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

// 🔹 JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key fehlt in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 🔹 Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Standard: /swagger
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

// 🔹 Datenbank-Migrationen beim Start automatisch anwenden (Dev-Komfort).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ALLE Endpoints hier
app.RegisterEndpoints();

app.Run();
