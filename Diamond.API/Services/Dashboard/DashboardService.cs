using Diamond.API.Repositories.Dashboard;
using Diamond.Share.Models;

namespace Diamond.API.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {

        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository repo) => _repo = repo;

        public Task<DashboardSummary> GetDashboardAsync() => _repo.GetDashboardAsync();

    }
}
