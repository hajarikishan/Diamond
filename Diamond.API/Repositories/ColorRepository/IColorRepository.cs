using Diamond.Share.Models;

namespace Diamond.API.Repositories.ColorRepository
{
    public interface IColorRepository
    {
        Task<IEnumerable<MD_COLOR>> GetAllAsync();
        Task<MD_COLOR?> GetByIdAsync(int id);
        Task<MD_COLOR> CreateAsync(MD_COLOR color);
        Task<MD_COLOR?> UpdateAsync(int id, MD_COLOR color);
        Task<bool> DeleteAsync(int id);
    }
}
