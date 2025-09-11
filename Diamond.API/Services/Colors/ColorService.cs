using Diamond.API.Repositories.ColorRepository;
using Diamond.Share.Models;

namespace Diamond.API.Services.Colors
{
    public class ColorService : IColorService
    {

        private readonly IColorRepository _repo;

        public ColorService(IColorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MD_COLOR>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<MD_COLOR?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<MD_COLOR> CreateAsync(MD_COLOR color)
        {
            if (string.IsNullOrWhiteSpace(color.ColorName))
                throw new ArgumentException("ColorName is required.");

            color.Description ??= "";
            return await _repo.CreateAsync(color);
        }

        public async Task<MD_COLOR?> UpdateAsync(int id, MD_COLOR color)
        {
            if (string.IsNullOrWhiteSpace(color.ColorName))
                throw new ArgumentException("ColorName cannot be empty.");

            return await _repo.UpdateAsync(id, color);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

    }
}
