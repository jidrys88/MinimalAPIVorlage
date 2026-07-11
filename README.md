# MinimalAPIVorlage
Kurzbeschreibung: Diese Anwendung ist eine .NET 8 Minimal API Vorlage mit klarer Trennung von API, Service- und Datenzugriffsschicht. Sie nutzt Entity Framework Core mit SQLite, Generic Repository, DTOs mit Validierung und Swagger und ist leicht erweiterbar und testbar.

Minimal API Vorlage (.NET 8)
📌 Übersicht

Diese Lösung ist eine saubere Minimal API Architektur mit:

✅ .NET 8 Minimal API

✅ SQLite Datenbank

✅ Entity Framework Core

✅ Generic Repository

✅ Service Layer

✅ DTOs mit DataAnnotations-Validierung (kein Overposting)

✅ Generic Request / Response

✅ Swagger Integration


🔄 Architektur

Minimal API (EndPoints)

    ↓

Service (Business-Logik, DTO-Mapping, Validierung)

    ↓

Generic Repository

    ↓

DbContext (SQLite)

Hinweis: Die frühere separate DataHandler-Schicht wurde entfernt, da sie
lediglich 1:1 an das Repository durchgereicht hat, ohne eigenen Mehrwert.
Der Service greift jetzt direkt auf `IGenericRepository<T>` zu. Sobald echte
Zusatzlogik (z. B. Caching) benötigt wird, kann diese Schicht gezielt wieder
eingeführt werden.

📦 DTOs statt Entities im API-Vertrag

Die API gibt niemals die EF-Core-Entity direkt zurück/entgegen, sondern
eigene DTOs (`DataModels/Dtos`):

- `ProductRequestDto` – was beim Anlegen (POST) und Ändern (PUT) erlaubt ist
- `ProductDto` – was beim Lesen (GET) zurückgegeben wird

Dadurch ist das Domänenmodell (`Product`-Entity) vom öffentlichen API-Vertrag
entkoppelt, und Clients können keine Felder setzen, die nicht explizit im
jeweiligen DTO vorgesehen sind (Schutz vor Overposting). Validierung erfolgt
über DataAnnotations-Attribute auf den DTOs und wird zentral über
`Common.ValidationHelper` geprüft. Das Mapping zwischen Entity und DTOs
übernimmt `DataModels/Entities/ProductMappingExtensions.cs`.

🗄 Datenbank

SQLite

ConnectionString in appsettings.json:

```json
"ConnectionStrings": {
  "Default": "Data Source=app.db"
}
```

Migration erstellen (sobald sich das Datenmodell ändert):

```
dotnet ef migrations add <Name> --project DBUmgebung --startup-project MinimalAPIVorlage
dotnet ef database update --project DBUmgebung --startup-project MinimalAPIVorlage
```

🚀 Projekt starten

```
dotnet run --project MinimalAPIVorlage
```

Swagger UI:

https://localhost:{port}/swagger

📦 Beispiel Endpoints

GET /products

GET /products/{id}

POST /products

PUT /products/{id}

DELETE /products/{id}

Beispiel-Body für POST/PUT:

```json
{
  "data": {
    "name": "Testprodukt",
    "price": 19.99
  }
}
```

📌 Technologien

.NET 8

Entity Framework Core

SQLite

Swashbuckle (Swagger)
