using Diamond.API.Services.Polish;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolishController : ControllerBase
    {
        private readonly IPolishService _service;

        public PolishController(IPolishService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var polish = await _service.GetAllAsync();
            return Ok(polish);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var polish = await _service.GetByIdAsync(id);
            return polish is not null ? Ok(polish) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_POLISH polish)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(polish);
            return CreatedAtAction(nameof(GetById), new { id = created.PolishId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MD_POLISH polish)
        {
            var updated = await _service.UpdateAsync(id, polish);
            return updated is not null ? Ok(updated) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
