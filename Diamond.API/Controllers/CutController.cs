using Diamond.API.Services.Cut;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CutController : ControllerBase
    {
        private readonly ICutService _service;

        public CutController(ICutService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cut = await _service.GetAllAsync();
            return Ok(cut);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cut = await _service.GetByIdAsync(id);
            return cut is not null ? Ok(cut) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_CUT cut)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(cut);
            return CreatedAtAction(nameof(GetById), new { id = created.CutId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MD_CUT cut)
        {
            var updated = await _service.UpdateAsync(id, cut);
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
