using Diamond.Share.Models;
using Diamond.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly IColorRepository _repo;

        public ColorsController(IColorRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var color = await _repo.GetByIdAsync(id);
            return color is not null ? Ok(color) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_COLOR color)
        {
            if (string.IsNullOrWhiteSpace(color.ColorName))
                return BadRequest("ColorName is required.");

            var created = await _repo.CreateAsync(color);
            return CreatedAtAction(nameof(GetById), new { id = created.ColorId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MD_COLOR color)
        {
            var updated = await _repo.UpdateAsync(id, color);
            return updated is not null ? Ok(updated) : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
