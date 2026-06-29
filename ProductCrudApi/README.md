# Product CRUD API

A RESTful Web API for managing Products and Items, built with ASP.NET Core, Entity Framework Core, and JWT Authentication.

## Tech Stack

- **.NET 10 / ASP.NET Core Web API**
- **Entity Framework Core** with **SQLite**
- **JWT Bearer Authentication**
- **FluentValidation** for request validation
- **Swagger / Swashbuckle** for API documentation
- **BCrypt.Net** for password hashing

## Project Structure
ProductCrudApi/

├── Controllers/         # API endpoints (Auth, Products, Items)

├── Domain/

│   └── Entities/         # Product, Item, User

├── Application/

│   ├── DTOs/              # Request/Response models

│   ├── Interfaces/        # Service contracts

│   └── Validators/        # FluentValidation rules

├── Infrastructure/

│   ├── Data/              # ApplicationDbContext (EF Core)

│   └── Identity/          # JWT TokenService

└── Program.cs            # App configuration & startup

## Database Schema

**Product**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| ProductName | string | Required, max 255 chars |
| CreatedBy | string | Required |
| CreatedOn | datetime | Required |
| ModifiedBy | string | Nullable |
| ModifiedOn | datetime | Nullable |

**Item**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| ProductId | int | Foreign Key → Product |
| Quantity | int | Required |

## How to Run Locally

1. Clone the repository
```bash
   git clone https://github.com/Rohan-007-lab/ProductCrudApi.git
```
2. Open the solution in Visual Studio
3. Restore NuGet packages (Visual Studio does this automatically on build)
4. Run EF Core migrations to create the SQLite database:
```powershell
   Add-Migration InitialCreate
   Update-Database
```
5. Run the project (F5) — Swagger UI will open automatically

## Authentication

All `Products` and `Items` endpoints require a valid JWT token.

1. Register a new user → `POST /api/auth/register`
2. Login → `POST /api/auth/login`
3. Copy the returned token
4. In Swagger, click **Authorize** and enter: `Bearer <your-token>`

## API Endpoints

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and get JWT token |

### Products
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Get all products with items |
| GET | `/api/products/{id}` | Get a single product |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

### Items
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/items` | Get all items |
| GET | `/api/items/{id}` | Get a single item |
| GET | `/api/products/{productId}/items` | Get items for a product |
| POST | `/api/items` | Create a new item |
| PUT | `/api/items/{id}` | Update item quantity |
| DELETE | `/api/items/{id}` | Delete an item |

## Sample Requests

**Register**
```json
POST /api/auth/register
{
  "username": "rohan",
  "password": "Test@123"
}
```

**Login**
```json
POST /api/auth/login
{
  "username": "rohan",
  "password": "Test@123"
}
```

**Create Product**
```json
POST /api/products
{
  "productName": "Wireless Mouse"
}
```

**Create Item**
```json
POST /api/items
{
  "productId": 1,
  "quantity": 50
}
```

## Notes

- Database used: **SQLite** (file-based, no separate installation required)
- The JWT secret key in `appsettings.json` is for demonstration purposes only. In production, it should be stored securely (e.g., environment variables or a secrets manager).

## Author

**Rohan Kale**
Email: rohankale7722@gmail.com