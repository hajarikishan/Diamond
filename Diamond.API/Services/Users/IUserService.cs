using Diamond.Share.Models;

namespace Diamond.API.Services.Users
{
    public interface IUserService
    {

        Task<MD_USER> RegisterAsync(MD_USER user, string password);
        Task<MD_USER?> ValidateUserAsync(string email, string password);
        Task<bool> SetResetTokenAsync(string email, string token, DateTime expiry);
        Task<MD_USER?> GetByResetTokenAsync(string token);
        Task<bool> ResetPasswordAsync(int userId, string newPassword);
        Task<MD_USER?> GetByIdAsync(int userId);

    }
}
