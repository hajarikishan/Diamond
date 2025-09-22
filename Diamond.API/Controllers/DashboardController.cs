using Diamond.API.Repositories.Dashboard;
using Diamond.Share.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _repo;

        public DashboardController(DashboardRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardSummary>> GetSummary()
        {
            var result = await _repo.GetSummaryAsync();
            return Ok(result);
        }
    }
}
