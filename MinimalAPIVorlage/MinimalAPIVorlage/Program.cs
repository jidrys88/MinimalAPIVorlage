using DataHandler;
using DataModels;
using DBUmgebung;
using Microsoft.EntityFrameworkCore;
using MinimalAPIVorlage;
using Services;
using SQLitePCL;



var builder = WebApplication.CreateBuilder(args);

// DBUmgebung
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=MyDatabase.db"));

// DataHandler
builder.Services.AddScoped(typeof(IEntityDataHandler<>), typeof(EntityDataHandler<>));

// Services
builder.Services.AddScoped(typeof(IEntityService<,>), typeof(EntityService<,>));

// 🔹 Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// 🔹 Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Standard: /swagger
}

// Endpoints direkt registrieren (Scoped Service wird automatisch per DI injiziert)
app.MapEntityEndpoints();

app.Run();
