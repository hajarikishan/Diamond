using Diamond.Share.Models;

namespace Diamond.API.Repositories.Clarity
{
    public interface IClarityRepository
    {

        Task<IEnumerable<MD_CLARITY>> GetAllAsync();
        Task<MD_CLARITY?> GetByIdAsync(int id);
        Task<int> AddAsync(MD_CLARITY item);
        Task<bool> UpdateAsync(MD_CLARITY item);
        Task<bool> DeleteAsync(int id);

    }
}
