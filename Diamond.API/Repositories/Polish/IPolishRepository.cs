using Diamond.Share.Models;

namespace Diamond.API.Repositories.Polish
{
    public interface IPolishRepository
    {

        Task<IEnumerable<MD_POLISH>> GetAllAsync();
        Task<MD_POLISH?> GetByIdAsync(int id);
        Task<int> AddAsync(MD_POLISH item);
        Task<bool> UpdateAsync(MD_POLISH item);
        Task<bool> DeleteAsync(int id);

    }
}
