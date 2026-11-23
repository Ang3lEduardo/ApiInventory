# Inventory API

An ASP.NET Core Web API for managing product categories, products, and inventory movements. The API targets .NET 8 and stores data in SQL Server.

## Features

- Create, read, update, and delete categories
- Create, read, update, and delete products
- Record inventory movements and retrieve movement history by product
- Entity Framework Core for database initialization and persistence, with Dapper for quering data operations
- OpenAPI documentation through Swagger

## Requirements

- .NET 8 SDK
- SQL Server

The API applies EF Core migrations at startup and adds sample categories and products when the category table is empty. Make sure the configured SQL Server is reachable and the database user can create or update the schema.

## Configuration

The API reads its SQL Server connection string from `ConnectionStrings:InventoryDb`. JWT Bearer settings are read from `Authentication:Authority` and `Authentication:Audience`.

For local development, these values can be supplied as environment variables:

```bash
export ConnectionStrings__InventoryDb='Server=localhost;Database=InventoryDb;User Id=<user>;Password=<password>;TrustServerCertificate=True;'
export Authentication__Authority='https://identity.example.com/<tenant>'
export Authentication__Audience='inventory-api'
```

Use the issuer URL and audience configured for your chosen identity provider and API. Do not commit credentials or production secrets to source control.

## Authentication

The API is configured to use OAuth 2.0 Bearer access tokens. ASP.NET Core JWT Bearer authentication is used to validate tokens issued by an external OAuth 2.0/OpenID Connect identity provider.

The technical requirements do not specify a particular identity provider, so the issuer (`Authority`) and API audience are configurable and are not coupled to a specific vendor. Configure a concrete provider through application settings when deploying the API.

**Current implementation note:** JWT Bearer validation is configured, but the API currently does not call `UseAuthentication()` or apply authorization policies/`[Authorize]` to its controllers. As a result, endpoints are not currently protected by bearer-token enforcement. Enable the authentication middleware and endpoint authorization before relying on the API to require tokens.

## Run locally

From the repository root, configure the settings above and run:

```bash
dotnet run --project src/Inventory.Api/Inventory.Api.csproj
```

With the `http` launch profile, the API listens at `http://localhost:5295`. Swagger UI is available at `http://localhost:5295/swagger`.

## API endpoints

All routes are prefixed with `/api`.

| Resource | Method | Route | Description |
| --- | --- | --- | --- |
| Categories | `GET` | `/api/categories` | List categories |
| Categories | `GET` | `/api/categories/{id}` | Get a category |
| Categories | `POST` | `/api/categories` | Create a category |
| Categories | `PUT` | `/api/categories/{id}` | Update a category |
| Categories | `DELETE` | `/api/categories/{id}` | Delete a category |
| Products | `GET` | `/api/products` | List products |
| Products | `GET` | `/api/products/{id}` | Get a product |
| Products | `POST` | `/api/products` | Create a product |
| Products | `PUT` | `/api/products/{id}` | Update a product |
| Products | `DELETE` | `/api/products/{id}` | Delete a product |
| Inventory movements | `GET` | `/api/inventory-movements/product/{productId}` | List movements for a product |
| Inventory movements | `POST` | `/api/inventory-movements` | Record an inventory movement |

Example create-category request:

```http
POST /api/categories
Content-Type: application/json

{
  "name": "Office Supplies"
}
```

Example create-product request:

```http
POST /api/products
Content-Type: application/json

{
  "name": "Wireless Mouse",
  "categoryId": 1,
  "stock": 25
}
```

Example inventory-movement request:

```http
POST /api/inventory-movements
Content-Type: application/json

{
  "productId": 1,
  "quantity": 5,
  "movementType": 1
}
```

The `movementType` value is the integer representation of the `MovementType` enum.

## Tests

Run the test projects from the repository root:

```bash
dotnet test tests/Inventory.Domain.Tests/Inventory.Domain.Tests.csproj
dotnet test tests/Inventory.Application.Tests/Inventory.Application.Tests.csproj
dotnet test tests/Inventory.IntegrationTests/Inventory.IntegrationTests.csproj
```
