using Diamond.API.Services.Clarity;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClarityController : ControllerBase
    {
        private readonly IClarityService _service;

        public ClarityController(IClarityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clarity = await _service.GetAllAsync();
            return Ok(clarity);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var clarity = await _service.GetByIdAsync(id);
            return clarity is not null ? Ok(clarity) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_CLARITY clarity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(clarity);
            return CreatedAtAction(nameof(GetById), new { id = created.ClarityId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MD_CLARITY clarity)
        {
            var updated = await _service.UpdateAsync(id, clarity);
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
