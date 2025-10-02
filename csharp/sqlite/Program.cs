using AuthCSharpSQLite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();
builder.Configuration.AddEnvironmentVariables();

// DbContext SQLite
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication + Blacklist Validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] 
                    ?? "fallbackKeyWithAtLeast32CharsLength1234567890"))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<AuthDbContext>();
                var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    var revoked = await db.RevokedTokens.AnyAsync(r => r.Jti == jti);
                    if (revoked)
                    {
                        context.Fail("Token has been revoked");
                    }
                }
            }
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();