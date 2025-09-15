using Diamond.Share.Models;

namespace Diamond.API.Services.Clarity
{
    public interface IClarityService
    {

        Task<IEnumerable<MD_CLARITY>> GetAllAsync();
        Task<MD_CLARITY?> GetByIdAsync(int id);
        Task<MD_CLARITY> CreateAsync(MD_CLARITY item);
        Task<MD_CLARITY?> UpdateAsync(int id, MD_CLARITY item);
        Task<bool> DeleteAsync(int id);

    }
}
