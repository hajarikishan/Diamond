using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models;

namespace Diamond.API.Repositories.Polish
{
    public class PolishRepository : IPolishRepository
    {

        private readonly DapperContext _context;

        public PolishRepository(DapperContext context) => _context = context;

        public async Task<IEnumerable<MD_POLISH>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT PolishId, PolishName, Description, Rank FROM MD_POLISH ORDER BY Rank, PolishName";
            return await conn.QueryAsync<MD_POLISH>(sql);
        }

        public async Task<MD_POLISH?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT PolishId, PolishName, Description, Rank FROM MD_POLISH WHERE PolishId=@Id";
            return await conn.QueryFirstOrDefaultAsync<MD_POLISH>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(MD_POLISH item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO MD_POLISH (PolishName, Description, Rank)
                        VALUES (@PolishName, @Description, @Rank);
                        SELECT IDENT_CURRENT('MD_POLISH');";
            return await conn.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task<bool> UpdateAsync(MD_POLISH item)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_POLISH SET
                        PolishName=@PolishName, Description=@Description, Rank=@Rank
                        WHERE PolishId=@PolishId";
            var rows = await conn.ExecuteAsync(sql, item);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "DELETE FROM MD_POLISH WHERE PolishId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
