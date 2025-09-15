using Diamond.Share.Models;

namespace Diamond.API.Services.Cut
{
    public interface ICutService
    {

        Task<IEnumerable<MD_CUT>> GetAllAsync();
        Task<MD_CUT?> GetByIdAsync(int id);
        Task<MD_CUT> CreateAsync(MD_CUT item);
        Task<MD_CUT?> UpdateAsync(int id, MD_CUT item);
        Task<bool> DeleteAsync(int id);


    }
}
