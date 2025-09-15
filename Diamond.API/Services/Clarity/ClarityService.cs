using Diamond.API.Repositories.Clarity;
using Diamond.Share.Models;

namespace Diamond.API.Services.Clarity
{
    public class ClarityService : IClarityService
    {

        private readonly IClarityRepository _repo;

        public ClarityService(IClarityRepository repo) => _repo = repo;

        public Task<IEnumerable<MD_CLARITY>> GetAllAsync() => _repo.GetAllAsync();

        public Task<MD_CLARITY?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<MD_CLARITY> CreateAsync(MD_CLARITY item)
        {
            if (string.IsNullOrWhiteSpace(item.ClarityName))
                throw new ArgumentException("ClarityName is required");

            item.Description ??= "";
            var id = await _repo.AddAsync(item);
            item.ClarityId = id;
            return item;
        }

        public async Task<MD_CLARITY?> UpdateAsync(int id, MD_CLARITY item)
        {
            if (string.IsNullOrWhiteSpace(item.ClarityName))
                throw new ArgumentException("ClarityName is required");

            item.ClarityId = id;
            var ok = await _repo.UpdateAsync(item);
            return ok ? item : null;
        }
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
