using Diamond.Share.Models;

namespace Diamond.API.Services.Shapes
{
    public interface IShapeService
    {

        Task<IEnumerable<MD_SHAPE>> GetAllAsync();
        Task<MD_SHAPE?> GetByIdAsync(int id);
        Task<MD_SHAPE> AddAsync(MD_SHAPE shape);
        Task<MD_SHAPE?> UpdateAsync(int id, MD_SHAPE shape);
        Task<bool> DeleteAsync(int id);

    }
}
