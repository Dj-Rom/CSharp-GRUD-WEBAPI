# CSharp-GRUD-WEBAPI
# CRUDWebApi

ASP.NET Core 10 REST API with JWT authentication and Scalar UI.

---

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- JWT Bearer Authentication
- Scalar (API documentation UI)

---

## Project Structure

```
CRUDWebApi/
├── Controllers/
│   ├── AuthController.cs       # Login, token generation
│   └── UserController.cs       # CRUD for users
├── Model/
│   └── User.cs                 # User model
├── appsettings.json            # Configuration (JWT keys, etc.)
└── Program.cs                  # App entry point, middleware pipeline
```

---

## Getting Started

### 1. Clone and run

```bash
git clone <repo-url>
cd CRUDWebApi
dotnet run
```

### 2. Open Scalar UI

```
https://localhost:{port}/scalar/v1
```

---

## Configuration

`appsettings.json`:

```json
{
  "Jwt": {
    "Key": "supersecretkey1234567890123456!!",
    "Issuer": "myapp",
    "Audience": "myusers"
  }
}
```

> ⚠️ Key must be **minimum 32 characters**. Never commit real keys to git — use environment variables in production.

---

## Authentication

The API uses **JWT Bearer** tokens.

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Using the token

Add the token to every protected request:

```http
Authorization: Bearer <your_token>
```

### Token details

| Property  | Value         |
|-----------|---------------|
| Algorithm | HMAC SHA-256  |
| Issuer    | myapp         |
| Audience  | myusers       |
| Lifetime  | 1 hour        |
| Role      | Admin         |

---

## API Endpoints

### Auth

| Method | Endpoint          | Auth required | Description        |
|--------|-------------------|---------------|--------------------|
| POST   | /api/auth/login   | ❌ No          | Get JWT token      |

### Users

| Method | Endpoint          | Auth required | Role  | Description         |
|--------|-------------------|---------------|-------|---------------------|
| GET    | /api/user         | ✅ Yes         | Any   | Get all users       |
| GET    | /api/user/{id}    | ✅ Yes         | Any   | Get user by ID      |
| POST   | /api/user         | ✅ Yes         | Any   | Create user         |
| PUT    | /api/user/{id}    | ✅ Yes         | Any   | Update user         |
| DELETE | /api/user/{id}    | ✅ Yes         | Admin | Delete user         |

---

## User Model

```json
{
  "id": 1,
  "firstName": "Ivan",
  "lastName": "Petrov",
  "email": "ivan@mail.com",
  "phoneNumber": "123123123"
}
```

### Validation rules

- `firstName` — required
- `lastName` — required
- `email` — required, stored as lowercase
- `phoneNumber` — optional, spaces are removed automatically

---

## Authorization

All endpoints are **protected by default** via global `AuthorizeFilter`.

- `POST /api/auth/login` — public (`[AllowAnonymous]`)
- `DELETE /api/user/{id}` — Admin role only (`[Authorize(Roles = "Admin")]`)
- All other endpoints — any authenticated user

### How roles work

The `Admin` role is assigned during token generation in `AuthController`. In a real app, roles should come from a database.

---

## Scalar UI

Scalar replaces Swagger in .NET 10. Available in **Development** mode only.

**URL:** `https://localhost:{port}/scalar/v1`

You can test all endpoints directly in the browser, including sending the Bearer token.

---

## Middleware Pipeline

```
Request
  └── UseHttpsRedirection
  └── UseAuthentication     ← who are you?
  └── UseAuthorization      ← what can you do?
  └── MapControllers
```

> ⚠️ Order matters — `UseAuthentication` must always come before `UseAuthorization`.

---

## Notes

- User data is stored **in memory** (`static List<User>`). Data resets on every restart.
- For production, replace with a real database (Entity Framework Core + PostgreSQL/SQL Server).
- JWT key should be stored in environment variables or Azure Key Vault, not in `appsettings.json`.

---

## Future Improvements

- [ ] Connect real database (EF Core)
- [ ] User registration endpoint
- [ ] Password hashing (BCrypt)
- [ ] Refresh tokens
- [ ] Role management from DB
- [ ] Docker support