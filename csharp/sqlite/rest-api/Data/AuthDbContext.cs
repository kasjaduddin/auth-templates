using Microsoft.EntityFrameworkCore;
using AuthCSharpSQLite.Models;

namespace AuthCSharpSQLite.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }
    }
}