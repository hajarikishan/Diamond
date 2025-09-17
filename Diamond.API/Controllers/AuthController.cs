using Diamond.API.Services;
using Diamond.API.Services.Users;
using Diamond.Share.Models;
using Diamond.API.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userSvc;
        private readonly TokenService _tokenSvc;

        public AuthController(IUserService userSvc, TokenService tokenSvc)
        {
            _userSvc = userSvc;
            _tokenSvc = tokenSvc;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            try
            {
                var user = new MD_USER
                {
                    UserName = req.UserName,
                    Email = req.Email,
                    Role = req.Role ?? "User"
                };
                var created = await _userSvc.RegisterAsync(user, req.Password);
                return CreatedAtAction(nameof(Register), new { id = created.UserId }, new { created.UserId, created.Email, created.UserName });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userSvc.ValidateUserAsync(req.Email, req.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            var token = _tokenSvc.CreateToken(user);
            return Ok(new { token, user = new { user.UserId, user.UserName, user.Email, user.Role } });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            //generate token
            var token = Guid.NewGuid().ToString("N");
            var expiry = DateTime.UtcNow.AddHours(2);

            var ok = await _userSvc.SetResetTokenAsync(req.Email, token, expiry);

            if (!ok) return NotFound();

            return Ok(new { message = "Reset token generated (dev only)", token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var user = await _userSvc.GetByResetTokenAsync(req.Token);
            if (user == null) return BadRequest("Invalid or expired token");

            var ok = await _userSvc.ResetPasswordAsync(user.UserId, req.NewPassword);
            return ok ? Ok() : StatusCode(500, "Unable to reset password");
        }
    }
}
