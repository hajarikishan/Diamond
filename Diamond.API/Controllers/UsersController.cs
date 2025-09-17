using Diamond.API.Repositories.Users;
using Diamond.API.Services.Users;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System.Security.Claims;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userSvc;
        private readonly IUserRepository _repo;
        private readonly IWebHostEnvironment _env;

        public UsersController(IUserService userSvc, IUserRepository repo, IWebHostEnvironment env)
        {
            _userSvc = userSvc;
            _repo = repo;
            _env = env;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var uid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userSvc.GetByIdAsync(uid);

            if (user == null) return NotFound();
            return Ok(new { user.UserId, user.UserName, user.Email, user.Role, user.ProfilePicturePath });
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] MD_USER model)
        {
            var uid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            model.UserId = uid;
            model.PasswordHash = null!;
            var ok = await _repo.UpdateAsync(model);
            return ok ? Ok() : StatusCode(500);
        }

        [HttpPost("{id}/avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar(int id, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("File Missing");

            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "users");
            Directory.CreateDirectory(uploads);

            var ext = Path.GetExtension(file.FileName);
            var filename = $"{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(uploads, filename);

            using var fs = new FileStream(full, FileMode.Create);
            await file.CopyToAsync(fs);

            var urlPath = $"/uploads/users/{filename}";
            var ok = await _repo.UpdateProfilePictureAsync(id, urlPath);
            return ok ? Ok(new { url = urlPath }) : StatusCode(500);
        }

    }
}
