using Diamond.WebUI.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace Diamond.WebUI.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private readonly IJSRuntime _js;

        public CustomAuthStateProvider(IJSRuntime js) => _js = js;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            ClaimsPrincipal user = JwtParser.ParseClaimsFromJwt(token);

            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = JwtParser.ParseClaimsFromJwt(token);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
