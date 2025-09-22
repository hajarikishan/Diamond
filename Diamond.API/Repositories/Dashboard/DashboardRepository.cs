using System.Data;
using Dapper;
using Diamond.Share.Models.Dashboard;

namespace Diamond.API.Repositories.Dashboard
{
    public class DashboardRepository
    {
        private readonly IDbConnection _db;

        public DashboardRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<DashboardSummary> GetSummaryAsync()
        {
            var summary = new DashboardSummary
            {
                ColorsCount = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM MD_COLOR"),
                ShapesCount = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM MD_SHAPE"),
                ClarityCount = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM MD_CLARITY"),
                CutCount = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM MD_CUT"),
                PurityCount = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM MD_PURITY"),
                PolishCount = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM MD_POLISH"),
                LastUpdated = DateTime.UtcNow
            };

            return summary;
        }
    }
}
