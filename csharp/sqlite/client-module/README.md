# AuthClientModule

**AuthClientModule** is a reusable C# authentication client library designed to connect with the `rest-api/` (ASP.NET Core + SQLite).  
It provides a simple wrapper for calling authentication endpoints (`register`, `login`, `logout`, and secure API calls).  
This module can be integrated into **Unity**, **Godot (C#)**, or **Blazor** projects, and serves as a reference for JavaScript frameworks like **React**.

---

## 🚀 Build Instructions

1. Navigate to the `client-module/` folder:
   ```bash
   cd auth-templates/csharp/sqlite/client-module
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. The compiled DLL will be available at:
   ```
   bin/Debug/net8.0/AuthClientModule.dll
   ```

---

## 🎮 Unity Integration

1. Copy `AuthClientModule.dll` into:
   ```
   UnityProject/Assets/Plugins/
   ```

2. Example usage in Unity:
   ```csharp
   using UnityEngine;
   using AuthClientModule;

   public class AuthExample : MonoBehaviour
   {
       private async void Start()
       {
           var auth = new AuthService("https://localhost:5001/");

           var token = await auth.LoginAsync("testuser", "secret123");
           Debug.Log("Token: " + token);

           var result = await auth.CallSecureEndpointAsync("api/test/secure");
           Debug.Log("Secure endpoint response: " + result);
       }
   }
   ```

---

## 🎮 Godot (C#) Integration

1. Copy `AuthClientModule.dll` into:
   ```
   GodotProject/res://lib/
   ```

2. Example usage in Godot:
   ```csharp
   using Godot;
   using AuthClientModule;

   public partial class AuthExample : Node
   {
       public override async void _Ready()
       {
           var auth = new AuthService("https://localhost:5001/");

           var token = await auth.LoginAsync("testuser", "secret123");
           GD.Print("Token: " + token);

           var result = await auth.CallSecureEndpointAsync("api/test/secure");
           GD.Print("Secure endpoint response: " + result);
       }
   }
   ```

---

## 🌐 Web App Examples

### Blazor (C#)

Register `AuthService` in `Program.cs`:
```csharp
builder.Services.AddScoped(sp => new AuthService("https://localhost:5001/"));
```

Use it in a Razor component:
```csharp
@page "/login"
@inject AuthClientModule.AuthService Auth

<input @bind="username" placeholder="Username" />
<input @bind="password" type="password" />
<button @onclick="DoLogin">Login</button>

<p>@message</p>

@code {
    private string username;
    private string password;
    private string message;

    private async Task DoLogin()
    {
        var token = await Auth.LoginAsync(username, password);
        message = $"Token: {token}";
    }
}
```

### React (JavaScript)

Call the REST API directly:
```javascript
export async function login(username, password) {
  const response = await fetch("https://localhost:5001/api/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password })
  });
  const data = await response.json();
  return data.token;
}
```

---

## 📌 Notes

- Ensure the `rest-api/` project is running before testing the client module.
- `AuthClientModule` does not store user data; it only manages authentication requests and tokens.
- For distribution, share `AuthClientModule.dll` along with this documentation.

---

## 🗂 Project Structure

```
csharp/sqlite/
 ├── rest-api/        # ASP.NET Core Web API
 └── client-module/   # Reusable C# client library
```

---