using Diamond.API.Repositories;
using Diamond.API.Services.Email;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Diamond.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repo;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UserController(UserRepository repo, IEmailService emailService, IConfiguration config)
        {
            _repo = repo;
            _emailService = emailService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            var existingUser = await _repo.GetByUserName(request.UserName);
            if (existingUser != null) return BadRequest("Username already exists");

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                RoleId = request.RoleId
            };

            await _repo.CreateUser(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _repo.GetByUserName(request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password");

            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = new
                {
                    user.UserId,
                    user.UserName,
                    user.FullName,
                    user.RoleId,
                    role = new
                    {
                        roleId = user.Role?.RoleId,
                        roleName = user.Role?.RoleName
                    }
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // JWT is stateless; client discards token
            return Ok("Logged out");
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var user = await _repo.GetById(userId);
            if (user == null) return Unauthorized();
            return Ok(user);
        }

        [Authorize]
        [HttpPost("profile/upload")]
        public async Task<IActionResult> UploadProfileImage()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0) return BadRequest("No file uploaded");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/images/profiles/{fileName}";
            await _repo.UpdateProfileImage(userId, relativePath);

            var user = await _repo.GetById(userId);
            return Ok(user);
        }

        [Authorize]
        [HttpPost("profile/remove")]
        public async Task<IActionResult> RemoveProfileImage()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var user = await _repo.GetById(userId);
            if (user == null) return NotFound("User not found");

            if (!string.IsNullOrEmpty(user.ProfileImagePath))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            await _repo.UpdateProfileImage(userId, null);
            return Ok("Profile image removed");
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var user = await _repo.GetById(userId);
            if (user == null) return Unauthorized();

            if (!BCrypt.Net.BCrypt.Verify(req.OldPassword, user.PasswordHash))
                return BadRequest("Old password incorrect");

            var newHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            await _repo.ChangePassword(userId, newHash);
            return Ok("Password changed");
        }

        [HttpPost("forgot-request")]
        public async Task<IActionResult> ForgotRequest(ForgotRequest req)
        {
            var user = await _repo.GetByUserName(req.UserName);
            if (user == null) return BadRequest("User not found");

            var otp = new Random().Next(100000, 999999).ToString();
            var OtpHash = BCrypt.Net.BCrypt.HashPassword(otp);
            var resetToken = Guid.NewGuid().ToString();
            var expiresAt = DateTime.UtcNow.AddMinutes(10);

            await _repo.CreatePasswordResetRequest(user.UserId, resetToken, OtpHash, expiresAt);

            var html = $"<div style='font-family: Segoe UI; padding: 20px;'><h3>Password Reset OTP</h3><p>Your OTP is: <strong>{otp}</strong></p><p>It expires in 10 Minutes.</p></div>";
            await _emailService.SendAsync(user.UserName, "Password reset OTP", html);

            return Ok("OTP sent");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtp req)
        {
            var user = await _repo.GetByUserName(req.UserName);
            if (user == null) return BadRequest("User not found");

            var request = await _repo.GetPasswordResetRequestAsync(user.UserId);
            if (request == null) return BadRequest("No valid reset request");

            if (!BCrypt.Net.BCrypt.Verify(req.Otp, request.OtpHash)) return BadRequest("Invalid OTP");

            return Ok(new { ResetToken = request.ResetToken ?? Guid.NewGuid().ToString() });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword req)
        {
            var user = await _repo.GetByUserName(req.UserName);
            if (user == null) return BadRequest("User not found");

            var request = await _repo.GetPasswordResetRequestAsync(user.UserId, req.ResetToken);
            if (request == null) return BadRequest("Invalid or expired token");

            var newHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            await _repo.ChangePassword(user.UserId, newHash);

            await _repo.MarkResetRequestUsed(request.RequestId);
            return Ok("Password reset successful");
        }

        [HttpGet("/api/admin/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("/api/admin/update-user")]
        public async Task<IActionResult> UpdateUserRole(UpdateUser req)
        {
            await _repo.UpdateUserRoleAndActive(req.UserId, req.RoleId, req.IsActive);
            return Ok();
        }
    }
}
