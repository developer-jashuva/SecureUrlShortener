# Secure URL Shortener API

A secure and optimized URL shortening service built with ASP.NET Core and SQL Server.  
The system generates short links, prevents duplicate entries, tracks clicks, and blocks suspicious URLs.

---

## Features

- Secure URL shortening
- Duplicate URL detection (idempotent shortening)
- Redirect using short links
- Click analytics tracking
- URL history API
- Delete shortened URLs
- Suspicious link detection
- RESTful API with Swagger

---

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- LINQ
- Swagger / OpenAPI

---

## API Endpoints

### Shorten URL
**POST** `/api/url/shorten`

```json
{
  "originalUrl": "https://example.com"
}
```

---

### Redirect
**GET** `/{shortCode}`

---

### Get History
**GET** `/api/url/all`

---

### Delete URL
**DELETE** `/api/url/{code}`

---

## Architecture

Angular UI → ASP.NET Core API → SQL Server

---

## How to Run

```bash
git clone https://github.com/developer-jashuva/SecureUrlShortener
cd SecureUrlShortener
dotnet restore
dotnet ef database update
dotnet run
```

---

## Author

Sri Jashuva
