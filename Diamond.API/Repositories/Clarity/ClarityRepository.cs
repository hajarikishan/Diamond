using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models;

namespace Diamond.API.Repositories.Clarity
{
    public class ClarityRepository : IClarityRepository
    {

        private readonly DapperContext _context;
        public ClarityRepository(DapperContext context) => _context = context;

        public async Task<IEnumerable<MD_CLARITY>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT ClarityId, ClarityName, Abbreviation, Description, Rank FROM MD_CLARITY ORDER BY Rank, ClarityName";
            return await conn.QueryAsync<MD_CLARITY>(sql);
        }

        public async Task<MD_CLARITY?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT ClarityId, ClarityName, Abbreviation, Description, Rank FROM MD_CLARITY WHERE ClarityId = @Id";
            return await conn.QueryFirstOrDefaultAsync<MD_CLARITY>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(MD_CLARITY item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO MD_CLARITY (ClarityName, Abbreviation, Description, Rank)
                        VALUES (@ClarityName, @Abbreviation, @Description, @Rank);
                        SELECT IDENT_CURRENT('MD_CLARITY');";
            return await conn.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task<bool> UpdateAsync(MD_CLARITY item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_CLARITY SET
                        ClarityName=@ClarityName, Abbreviation=@Abbreviation, Description=@Description, Rank=@Rank
                        WHERE ClarityId=@ClarityId";
            var rows = await conn.ExecuteAsync(sql, item);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "DELETE FROM MD_CLARITY WHERE ClarityId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
