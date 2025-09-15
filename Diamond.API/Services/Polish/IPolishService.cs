using Diamond.Share.Models;

namespace Diamond.API.Services.Polish
{
    public interface IPolishService
    {

        Task<IEnumerable<MD_POLISH>> GetAllAsync();
        Task<MD_POLISH?> GetByIdAsync(int id);
        Task<MD_POLISH> CreateAsync(MD_POLISH item);
        Task<MD_POLISH?> UpdateAsync(int id, MD_POLISH item);
        Task<bool> DeleteAsync(int id);

    }
}
