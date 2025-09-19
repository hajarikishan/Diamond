using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Diamond.API.Models;
using Diamond.API.Repositories;
using Diamond.Share.Models.Auth;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<AuthResponse> RegisterAsync(RegisterRequest req)
        {
            var existing = await _userRepo.GetUserByEmailAsync(req.Email);
            if (existing != null) throw new Exception("Email already registered");

            var (salt, hash) = PasswordHasher.HashPassword(req.Password);

            var user = new User
            {
                Username = req.Username,
                Email = req.Email,
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = "User"
            };

            var id = await _userRepo.CreateUserAsync(user);
            user.Id = id;

            var token = GenerateJwtToken(user);
            return new AuthResponse { Token = token, RefreshToken = Guid.NewGuid().ToString(), Username = user.Username, Role = user.Role };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest req)
        {
            var user = await _userRepo.GetUserByEmailAsync(req.Email);
            if (user == null) throw new Exception("Invalid credentials");

            var ok = PasswordHasher.VerifyPassword(req.Password, user.PasswordSalt, user.PasswordHash);
            if (!ok) throw new Exception("Invalid credentials");

            var token = GenerateJwtToken(user);
            return new AuthResponse { Token = token, RefreshToken = Guid.NewGuid().ToString(), Username = user.Username, Role = user.Role };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
