using Diamond.Share.Models;

namespace Diamond.API.Repositories.Users
{
    public interface IUserRepository
    {

        Task<MD_USER?> GetByEmailAsync(string email);
        Task<MD_USER?> GetByIdAsync(int id);
        Task<int> CreateAsync(MD_USER user);
        Task<bool> UpdateAsync(MD_USER user);
        Task<bool> UpdateProfilePictureAsync(int userId, string path);
        Task<bool> SetPasswordResetTokenAsync(int userId, String token, DateTime expiry);
        Task<MD_USER?> GetByResetTokenAsync(string token);
        Task<bool> UpdatePasswordAsync(int userId, string newHash);

    }
}
