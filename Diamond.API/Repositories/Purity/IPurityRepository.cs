using Diamond.Share.Models;

namespace Diamond.API.Repositories.Purity
{
    public interface IPurityRepository
    {

        Task<IEnumerable<MD_PURITY>> GetAllAsync();
        Task<MD_PURITY?> GetByIdAsync(int id);
        Task<int> AddAsync(MD_PURITY item);
        Task<bool> UpdateAsync(MD_PURITY item);
        Task<bool> DeleteAsync(int id);

    }
}
