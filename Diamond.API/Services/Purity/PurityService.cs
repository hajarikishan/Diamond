using Diamond.API.Repositories.Purity;
using Diamond.Share.Models;

namespace Diamond.API.Services.Purity
{
    public class PurityService : IPurityService
    {
        private readonly IPurityRepository _repo;

        public PurityService(IPurityRepository repo) => _repo = repo;

        public Task<IEnumerable<MD_PURITY>> GetAllAsync() => _repo.GetAllAsync();

        public Task<MD_PURITY?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<MD_PURITY> CreateAsync(MD_PURITY item)
        {
            if (string.IsNullOrWhiteSpace(item.PurityName))
                throw new ArgumentException("PurityName is required");

            item.Description ??= "";
            var id = await _repo.AddAsync(item);
            item.PurityId = id;
            return item;
        }

        public async Task<MD_PURITY?> UpdateAsync(int id, MD_PURITY item)
        {
            if (string.IsNullOrWhiteSpace(item.PurityName))
                throw new ArgumentException("PurityName is required");

            item.PurityId = id;
            var ok = await _repo.UpdateAsync(item);
            return ok ? item : null;
        }
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
