using Diamond.API.Repositories.ShapeRepository;
using Diamond.Share.Models;

namespace Diamond.API.Services.Shapes
{
    public class ShapeService : IShapeService
    {

        private readonly IShapeRepository _repo;

        public ShapeService(IShapeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MD_SHAPE>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<MD_SHAPE?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<MD_SHAPE> AddAsync(MD_SHAPE shape)
        {
            var id = await _repo.AddAsync(shape);
            shape.ShapeId = id;
            return shape;
        }

        public async Task<MD_SHAPE?> UpdateAsync(int id, MD_SHAPE shape)
        {
            shape.ShapeId = id;
            var success = await _repo.UpdateAsync(shape);
            return success ? shape : null;
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
