using Dapper;
using Diamond.Share.Models;
using Diamond.API.Data;

namespace Diamond.API.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly DapperContext _context;

        public ColorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MD_COLOR>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT ColorId, ColorName, Description FROM MD_COLOR";
            return await conn.QueryAsync<MD_COLOR>(sql);
        }

        public async Task<MD_COLOR?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT ColorId, ColorName, Description FROM MD_COLOR WHERE ColorId = @Id";
            return await conn.QueryFirstOrDefaultAsync<MD_COLOR>(sql, new { Id = id });
        }

        public async Task<MD_COLOR> CreateAsync(MD_COLOR color)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO MD_COLOR (ColorName, Description) 
                        VALUES (@ColorName, @Description); 
                        SELECT IDENT_CURRENT('MD_COLOR');";
            color.ColorId = await conn.ExecuteScalarAsync<int>(sql, color);
            return color;
        }

        public async Task<MD_COLOR?> UpdateAsync(int id, MD_COLOR color)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_COLOR 
                        SET ColorName = @ColorName, Description = @Description 
                        WHERE ColorId = @Id";
            var rows = await conn.ExecuteAsync(sql, new { color.ColorName, color.Description, Id = id });
            if (rows < 1)
                return null;
            else
                return color;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "DELETE FROM MD_COLOR WHERE ColorId = @Id";
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
