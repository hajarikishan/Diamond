using Diamond.Share.Models;

namespace Diamond.API.Services.Dashboard
{
    public interface IDashboardService
    {

        Task<DashboardSummary> GetDashboardAsync();

    }
}
