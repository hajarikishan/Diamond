using Diamond.Share.Models;

namespace Diamond.API.Repositories.Dashboard
{
    public interface IDashboardRepository
    {

        Task<DashboardSummary> GetDashboardAsync();

    }
}
