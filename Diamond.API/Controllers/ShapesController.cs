using Diamond.API.Services.Shapes;
using Diamond.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShapesController : ControllerBase
    {
        private readonly IShapeService _service;

        public ShapesController(IShapeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shapes = await _service.GetAllAsync();
            return Ok(shapes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shape = await _service.GetByIdAsync(id);
            return shape is not null ? Ok(shape) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MD_SHAPE shape)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.AddAsync(shape);
            return CreatedAtAction(nameof(GetById), new { id = created.ShapeId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MD_SHAPE shape)
        {
            var updated = await _service.UpdateAsync(id, shape);
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
