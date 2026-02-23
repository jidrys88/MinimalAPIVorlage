# MinimalAPIVorlage
Kurzbeschreibung:  Diese Anwendung ist eine .NET 8 Minimal API Vorlage mit klarer Trennung von API, Service- und Datenzugriffsschicht. Sie nutzt Entity Framework Core mit SQLite, Generic Repository, DataHandler und Swagger und ist leicht erweiterbar und testbar.

Minimal API Vorlage (.NET 8)
📌 Übersicht

Diese Lösung ist eine saubere Minimal API Architektur mit:

✅ .NET 8 Minimal API

✅ SQLite Datenbank

✅ Entity Framework Core

✅ Generic Repository

✅ DataHandler Layer

✅ Service Layer

✅ Generic Request / Response

✅ Swagger Integration


🔄 Architektur
Minimal API

    ↓
    
Service (Business Logic)

    ↓
    
DataHandler (Datenzugriff)

    ↓
    
Generic Repository

    ↓
    
DbContext (SQLite)


🗄 Datenbank

SQLite

ConnectionString in appsettings.json

"ConnectionStrings": {
  "Default": "Data Source=app.db"
}

Migration ausführen:

dotnet ef migrations add InitialCreate
dotnet ef database update
🚀 Projekt starten
dotnet run

Swagger UI:

https://localhost:{port}/
📦 Beispiel Endpoint

GET /products

POST /products

📌 Technologien

.NET 8

Entity Framework Core

SQLite

Swashbuckle (Swagger)
