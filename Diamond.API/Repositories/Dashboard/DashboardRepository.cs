using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models;

namespace Diamond.API.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {

        private readonly DapperContext _context;

        public DashboardRepository(DapperContext context) => _context = context;

        public async Task<DashboardSummary> GetDashboardAsync()
        {
            var sql = @"SELECT  ColorsCount, ShapesCount, ClarityCount, CutCount, 
                                PurityCount, PolishCount, LastUpdated
                        FROM dbo.vw_DashboardCounts;";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<DashboardSummary>(sql);
            return result ?? new DashboardSummary { LastUpdated = DateTime.UtcNow };
        }


    }
}
