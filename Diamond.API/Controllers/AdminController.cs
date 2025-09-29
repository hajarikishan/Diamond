using Diamond.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.API.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly UserRepository _repo;

        public AdminController(UserRepository repo) => _repo = repo;

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var counts = await _repo.GetUserCountsByRole();
            return Ok(counts);
        }
    }
}
