using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthClientModule
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private string? _token;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // agar cocok meski casing JSON berbeda
        };

        public AuthService(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri(baseUrl) };
        }

        public async Task<string> RegisterAsync(string username, string password)
        {
            var payload = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/register", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var payload = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResponse>(json, _jsonOptions);

            _token = result?.Token;
            return _token;
        }

        public async Task<string> CallSecureEndpointAsync(string endpoint)
        {
            if (string.IsNullOrEmpty(_token))
                throw new System.Exception("You must login first.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task LogoutAsync()
        {
            if (string.IsNullOrEmpty(_token))
                return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.PostAsync("api/auth/logout", null);
            response.EnsureSuccessStatusCode();

            _token = null;
        }
    }
}