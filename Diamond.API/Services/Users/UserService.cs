using Diamond.API.Repositories.Users;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Identity;

namespace Diamond.API.Services.Users
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repo;
        private readonly PasswordHasher<MD_USER> _hasher = new PasswordHasher<MD_USER>();

        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<MD_USER> RegisterAsync(MD_USER user, string password)
        {
            var existing = await _repo.GetByEmailAsync(user.Email);
            if (existing != null) throw new ArgumentException("Email Already Registered");

            user.PasswordHash = _hasher.HashPassword(user, password);
            user.CreatedAt = DateTime.UtcNow;
            var id = await _repo.CreateAsync(user);
            user.UserId = id;
            return user;
        }

        public async Task<MD_USER?> ValidateUserAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return null;
            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return res == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<bool> SetResetTokenAsync(string email, string token, DateTime expiry)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return false;

            return await _repo.SetPasswordResetTokenAsync(user.UserId, token, expiry);
        }

        public async Task<MD_USER?> GetByResetTokenAsync(string token) => await _repo.GetByResetTokenAsync(token);

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            var dummy = new MD_USER(); //for hasher
            var newHash = _hasher.HashPassword(dummy, newPassword);
            return await _repo.UpdatePasswordAsync(userId, newHash);
        }
        public async Task<MD_USER?> GetByIdAsync(int userId) => await _repo.GetByIdAsync(userId);
    }
}
