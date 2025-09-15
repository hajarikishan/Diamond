using Diamond.API.Services.Purity;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurityController : ControllerBase
    {
        private readonly IPurityService _service;

        public PurityController(IPurityService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purity = await _service.GetAllAsync();
            return Ok(purity);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purity = await _service.GetByIdAsync(id);
            return purity is not null ? Ok(purity) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_PURITY purity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(purity);
            return CreatedAtAction(nameof(GetById), new { id = created.PurityId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MD_PURITY purity)
        {
            var updated = await _service.UpdateAsync(id, purity);
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
