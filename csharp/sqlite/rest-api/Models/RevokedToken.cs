namespace AuthCSharpSQLite.Models
{
    public class RevokedToken
    {
        public int Id { get; set; }
        public string Jti { get; set; } = string.Empty; // JWT ID
        public DateTime RevokedAt { get; set; } = DateTime.UtcNow;
    }
}