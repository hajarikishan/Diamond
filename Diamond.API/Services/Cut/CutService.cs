using Diamond.API.Repositories.Cut;
using Diamond.Share.Models;

namespace Diamond.API.Services.Cut
{
    public class CutService : ICutService
    {

        private readonly ICutRepository _repo;

        public CutService(ICutRepository repo) => _repo = repo;

        public Task<IEnumerable<MD_CUT>> GetAllAsync() => _repo.GetAllAsync();

        public Task<MD_CUT?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<MD_CUT> CreateAsync(MD_CUT item)
        {
            if (string.IsNullOrWhiteSpace(item.CutName))
                throw new ArgumentException("CutName is required");

            item.Description ??= "";
            var id = await _repo.AddAsync(item);
            item.CutId = id;
            return item;

        }

        public async Task<MD_CUT?> UpdateAsync(int id, MD_CUT item)
        {
            if (string.IsNullOrWhiteSpace(item.CutName))
                throw new ArgumentException("ClarityName is required");

            item.CutId = id;
            var ok = await _repo.UpdateAsync(item);
            return ok ? item : null;
        }
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
