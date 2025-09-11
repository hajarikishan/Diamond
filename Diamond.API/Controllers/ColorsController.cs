using Diamond.API.Services.Colors;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _service;

        public ColorsController(IColorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var color = await _service.GetByIdAsync(id);
            return color is not null ? Ok(color) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_COLOR color)
        {
            try
            {
                var created = await _service.CreateAsync(color);
                return CreatedAtAction(nameof(GetById), new { id = created.ColorId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MD_COLOR color)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, color);
                return updated is not null ? Ok(updated) : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
