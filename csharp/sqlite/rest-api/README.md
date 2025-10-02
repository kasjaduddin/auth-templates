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
/Controllers
  AuthController.cs
/Data
  AuthDbContext.cs
/Migrations
  ... (EF Core migration files)
/Program.cs
/appsettings.json
/.env
```

---

## ⚙️ Setup & Installation

### 1. Clone the repository
```bash
git clone https://github.com/kasjaduddin/auth-templates.git
cd csharp/sqlite
```

### 2. Create a `.env` file
At the root of the project, create a `.env` file:

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

## 🧪 Testing with Postman
1. **Register** a new user  
2. **Login** and copy the returned token  
3. Access a protected endpoint → should succeed  
4. **Logout** → token is blacklisted  
5. Try the protected endpoint again → should return `401 Unauthorized`

---

## 📌 Notes
- Use a JWT key of at least 32 characters for HS256 signing.  
- Secrets (JWT key, DB connection string) are managed via `.env`.  
- SQLite database file (`auth.db`) should not be committed to Git.  
- EF Core migrations are included so the schema can be rebuilt anywhere.

---

## 📜 License
This project is provided as-is for learning and template purposes. You are free to adapt and extend it for your own applications.

---
