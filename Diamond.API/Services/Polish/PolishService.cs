using Diamond.API.Repositories.Polish;
using Diamond.API.Services.Purity;
using Diamond.Share.Models;

namespace Diamond.API.Services.Polish
{
    public class PolishService : IPolishService
    {

        private readonly IPolishRepository _repo;

        public PolishService(IPolishRepository repo) => _repo = repo;

        public Task<IEnumerable<MD_POLISH>> GetAllAsync() => _repo.GetAllAsync();

        public Task<MD_POLISH?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<MD_POLISH> CreateAsync(MD_POLISH item)
        {
            if (string.IsNullOrWhiteSpace(item.PolishName))
                throw new ArgumentException("PolishName is required");

            item.Description ??= "";
            var id = await _repo.AddAsync(item);
            item.PolishId = id;
            return item;
        }

        public async Task<MD_POLISH?> UpdateAsync(int id, MD_POLISH item)
        {
            if (string.IsNullOrWhiteSpace(item.PolishName))
                throw new ArgumentException("PurityName is required");

            item.PolishId = id;
            var ok = await _repo.UpdateAsync(item);
            return ok ? item : null;
        }

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
