using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models;

namespace Diamond.API.Repositories.Cut
{
    public class CutRepository : ICutRepository
    {

        private readonly DapperContext _context;

        public CutRepository(DapperContext context) => _context = context;

        public async Task<IEnumerable<MD_CUT>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT CutId, CutName, Grade, Description, Score FROM MD_CUT ORDER BY CutName";
            return await conn.QueryAsync<MD_CUT>(sql);
        }

        public async Task<MD_CUT?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT CutId, CutName, Grade, Description, Score FROM MD_CUT WHERE CutId=@Id";
            return await conn.QueryFirstOrDefaultAsync<MD_CUT>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(MD_CUT item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO MD_CUT (CutName, Grade, Description, Score)
                        VALUES (@CutName, @Grade, @Description, @Score);
                        SELECT IDENT_CURRENT('MD_CUT');";
            return await conn.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task<bool> UpdateAsync(MD_CUT item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_CUT SET
                        CutName=@CutName, Grade=@Grade, Description=@Description, Score=@Score
                        WHERE CutId=@CutId";
            var rows = await conn.ExecuteAsync(sql, item);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "DELETE FROM MD_CUT WHERE CutId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
