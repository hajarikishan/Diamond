using Diamond.Share.Models.Auth;

namespace Diamond.WebUI.Services
{
    public class AuthService
    {

        private readonly HttpClient _http;

        public AuthService(HttpClient http) => _http = http;

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

    }
}
