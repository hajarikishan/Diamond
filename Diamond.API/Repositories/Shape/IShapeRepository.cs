using Diamond.Share.Models;

namespace Diamond.API.Repositories.ShapeRepository
{
    public interface IShapeRepository
    {

        Task<IEnumerable<MD_SHAPE>> GetAllAsync();
        Task<MD_SHAPE?> GetByIdAsync(int id);
        Task<int> AddAsync(MD_SHAPE shape);
        Task<bool> UpdateAsync(MD_SHAPE shape);
        Task<bool> DeleteAsync(int id);

    }
}
