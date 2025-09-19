using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Diamond.WebUI.Helpers
{
    public static class JwtParser
    {
        public static ClaimsPrincipal ParseClaimsFromJwt(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return new ClaimsPrincipal(new ClaimsIdentity());
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
    }
}
