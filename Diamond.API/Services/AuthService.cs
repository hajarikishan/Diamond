using Diamond.API.Repositories.User;
using Diamond.Share.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Diamond.API.Services
{
    public class AuthService
    {

        private readonly UserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(UserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        //Hash password with SHA256
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepo.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Email already registered");

            var hash = HashPassword(request.Password);
            await _userRepo.RegisterAsync(request, hash);

            return await LoginAsync(new LoginRequest
            {
                Email = request.Email,
                Password = request.Password
            });
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var storedHash = await _userRepo.GetPasswordHashAsync(request.Email);
            if (storedHash == null || storedHash != HashPassword(request.Password))
                throw new Exception("Invalid credentials");

            var user = await _userRepo.GetByEmailAsync(request.Email);
            return GenerateToken(user);
        }

        public AuthResponse GenerateToken(UserProfileDto user)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = Guid.NewGuid().ToString(), //later store in DB
                Role = user.Role,
                Username = user.Username
            };
        }
    }
}
