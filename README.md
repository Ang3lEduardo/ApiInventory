# Inventory API

An ASP.NET Core Web API for managing product categories, products, and inventory movements. The API targets .NET 8 and stores data in SQL Server.

## Features

- Create, read, update, and delete categories
- Create, read, update, and delete products
- Record inventory movements and retrieve movement history by product
- Entity Framework Core for database migrations and query operations, with Dapper for command operations
- OpenAPI documentation through Swagger
- Bearer-token authentication for API endpoints

## Requirements

- .NET 8 SDK
- SQL Server

At startup, the API applies EF Core migrations and inserts sample categories and products if the category table is empty. Make sure SQL Server is reachable and the configured database user can create or update the schema.

## Configuration

The API reads its SQL Server connection string from `ConnectionStrings:InventoryDb`. JWT Bearer settings are read from `Authentication:Authority` and `Authentication:Audience`. Configure all three values for your environment before starting the API.

For local development, these values can be supplied as environment variables:

```bash
export ConnectionStrings__InventoryDb='Server=localhost,1433;Database=InventoryDb;User Id=<user>;Password=<password>;TrustServerCertificate=True;'
export Authentication__Authority='https://identity.example.com/<tenant>'
export Authentication__Audience='inventory-api'
```

Use the issuer URL and audience configured for your chosen identity provider and API. Do not commit credentials or production secrets to source control.

## Authentication

The API is configured to use OAuth 2.0 Bearer access tokens. ASP.NET Core JWT Bearer authentication is used to validate tokens issued by an external OAuth 2.0/OpenID Connect identity provider.

The technical requirements do not specify a particular identity provider, so the issuer (`Authority`) and API audience are configurable and are not coupled to a specific vendor. Configure a concrete provider through application settings when deploying the API.

All category, product, and inventory-movement endpoints require a valid bearer access token. The API calls authentication and authorization middleware, and its controllers use `[Authorize]`. Obtain a token from the configured identity provider and send it as `Authorization: Bearer <access-token>`.

## Run locally

From the repository root, configure the settings above and run:

```bash
dotnet run --project src/Inventory.Api/Inventory.Api.csproj
```

With the `http` launch profile, the API listens at `http://localhost:5295`. Swagger UI is available at `http://localhost:5295/swagger`.

## Run with Docker Compose

As part of the initial setup, create your local environment file, update its values, then build and start the API and SQL Server:

```bash
cp .env.example .env
```

Edit `.env` and replace the example values for `SA_PASSWORD`, `JWT_AUTHORITY`, and `JWT_AUDIENCE`. Then run:

```bash
docker compose up --build -d
```

The API and Swagger UI are available at `http://localhost:8080` and `http://localhost:8080/swagger`, respectively. Keep `.env` local; do not commit credentials or production secrets.

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

The `movementType` value is the integer representation of `MovementType`: `1` for `Increase` and `2` for `Decrease`. Decrease movements cannot exceed the product's available stock.

## Tests

Run the test projects from the repository root:

```bash
dotnet test tests/Inventory.Domain.Tests/Inventory.Domain.Tests.csproj
dotnet test tests/Inventory.Application.Tests/Inventory.Application.Tests.csproj
dotnet test tests/Inventory.IntegrationTests/Inventory.IntegrationTests.csproj
```
