using Diamond.Share.Models;

namespace Diamond.API.Repositories.Cut
{
    public interface ICutRepository
    {

        Task<IEnumerable<MD_CUT>> GetAllAsync();
        Task<MD_CUT?> GetByIdAsync(int id);
        Task<int> AddAsync(MD_CUT item);
        Task<bool> UpdateAsync(MD_CUT item);
        Task<bool> DeleteAsync(int id);

    }
}
