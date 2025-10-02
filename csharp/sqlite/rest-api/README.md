# 🔐 C# + SQLite JWT Authentication API

A simple REST API built with **.NET 8**, **Entity Framework Core (SQLite)**, and **JWT Authentication**.  
It includes **token revocation (blacklist)** so that logout is secure and previously issued tokens can no longer be used.

---

## ✨ Features
- User **registration** and **login**
- JWT token generation with `iss`, `aud`, and `jti` claims
- **Logout** endpoint that revokes tokens by storing their `jti` in a blacklist table
- Middleware validation that rejects blacklisted tokens
- Lightweight **SQLite** database
- Configuration via **`.env`** file (JWT key, issuer, audience, connection string)

---

## 📂 Project Structure
```
rest-api/
 ├── Controllers/
 │    └── AuthController.cs
 ├── Data/
 │    └── AuthDbContext.cs
 ├── Migrations/
 │    └── ... (EF Core migration files)
 ├── Program.cs
 ├── appsettings.json
 ├── .env
 └── auth.db (runtime, ignored by Git)
```

---

## ⚙️ Setup & Installation

### 1. Clone the repository
```bash
git clone https://github.com/kasjaduddin/auth-templates.git
cd csharp/sqlite/rest-api
```

### 2. Create a `.env` file
At the root of `rest-api/`, create a `.env` file:

```
CONNECTIONSTRINGS__DEFAULTCONNECTION=Data Source=auth.db
JWT__KEY=yoursuperlongsecurekeywithatleast32chars
JWT__ISSUER=AuthApp
JWT__AUDIENCE=AuthAppUsers
```

### 3. Apply database migrations
```bash
dotnet ef database update
```

### 4. Run the application
```bash
dotnet run
```

---

## 🔑 API Endpoints

### Register
`POST /api/auth/register`
```json
{
  "username": "user",
  "password": "secret123"
}
```

### Login
`POST /api/auth/login`
```json
{
  "username": "user",
  "password": "secret123"
}
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Logout
`POST /api/auth/logout`  
Headers:
```
Authorization: Bearer <token_from_login>
```

### Protected Endpoint (example)
`GET /api/test/secure`  
Headers:
```
Authorization: Bearer <token_from_login>
```

- Before logout → returns `200 OK`  
- After logout → returns `401 Unauthorized`

---

## 🧪 Testing
- Use **Postman** or the included `sqlite.http` file for quick API testing.  
- Or build the [`client-module/`](../client-module) and consume the API directly from Unity or Godot.

---

## 🔗 Related Projects
- [`client-module/`](../client-module): A reusable C# library for Unity and Godot that consumes this API.

---

## 📌 Notes
- Use a JWT key of at least 32 characters for HS256 signing.  
- Secrets (JWT key, DB connection string) are managed via `.env`.  
- SQLite database file (`auth.db`) should not be committed to Git.  
- EF Core migrations are included so the schema can be rebuilt anywhere.

---