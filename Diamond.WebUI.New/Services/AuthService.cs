using Diamond.Share.Models.Auth;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using RegisterRequest = Diamond.Share.Models.Auth.RegisterRequest;

namespace Diamond.WebUI.New.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<AuthResponse> Register(RegisterRequest req)
        {
            var res = await _http.PostAsJsonAsync("api/auth/register", req);
            res.EnsureSuccessStatusCode();
            var auth = await res.Content.ReadFromJsonAsync<AuthResponse>()!;
            await SaveToken(auth.Token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            return auth;
        }

        public async Task<AuthResponse> Login(Share.Models.Auth.LoginRequest req)
        {
            var res = await _http.PostAsJsonAsync("api/auth/login", req);
            res.EnsureSuccessStatusCode();
            var auth = await res.Content.ReadFromJsonAsync<AuthResponse>()!;
            await SaveToken(auth.Token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            return auth;
        }

        public async Task SaveToken(string token) => await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        public async Task RemoveToken() => await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }
}
