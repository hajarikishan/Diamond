using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models;

namespace Diamond.API.Repositories.Purity
{
    public class PurityRepository : IPurityRepository
    {

        private readonly DapperContext _context;

        public PurityRepository(DapperContext context) => _context = context;

        public async Task<IEnumerable<MD_PURITY>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT PurityId, PurityName, Description, Rank FROM MD_PURITY ORDER BY Rank, PurityName";
            return await conn.QueryAsync<MD_PURITY>(sql);
        }

        public async Task<MD_PURITY?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT PurityId, PurityName, Description, Rank FROM MD_PURITY WHERE PurityId=@Id";
            return await conn.QueryFirstOrDefaultAsync<MD_PURITY>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(MD_PURITY item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO MD_PURITY (PurityName, Description, Rank)
                        VALUES (@PurityName, @Description, @Rank);
                        SELECT IDENT_CURRENT('MD_PURITY');";
            return await conn.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task<bool> UpdateAsync(MD_PURITY item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_PURITY SET
                        PurityName=@PurityName, Description=@Description, Rank=@Rank
                        WHERE PurityId=@PurityId";
            var rows = await conn.ExecuteAsync(sql, item);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "DELETE FROM MD_PURITY WHERE PurityId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
