using Diamond.API.Services.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var summary = await _service.GetDashboardAsync();
            return Ok(summary);
        }
    }
}
