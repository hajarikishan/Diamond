using Diamond.Share.Models;

namespace Diamond.API.Services.Purity
{
    public interface IPurityService
    {

        Task<IEnumerable<MD_PURITY>> GetAllAsync();
        Task<MD_PURITY?> GetByIdAsync(int id);
        Task<MD_PURITY> CreateAsync(MD_PURITY item);
        Task<MD_PURITY?> UpdateAsync(int id, MD_PURITY item);
        Task<bool> DeleteAsync(int id);

    }
}
