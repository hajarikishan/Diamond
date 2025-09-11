using Dapper;
using Diamond.API.Repositories.ShapeRepository;
using Diamond.Share.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Diamond.API.Repositories.Shapes
{
    public class ShapeRepository : IShapeRepository
    {
        private readonly IDbConnection _conn;

        public ShapeRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public async Task<IEnumerable<MD_SHAPE>> GetAllAsync()
        {
            var sql = "SELECT ShapeId, ShapeName, Description, MinCarat, MaxCarat FROM MD_SHAPE";
            return await _conn.QueryAsync<MD_SHAPE>(sql);
        }

        public async Task<MD_SHAPE?> GetByIdAsync(int id)
        {
            var sql = "SELECT ShapeId, ShapeName, Description, MinCarat, MaxCarat FROM MD_SHAPE WHERE ShapeId = @Id";
            return await _conn.QueryFirstOrDefaultAsync<MD_SHAPE>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(MD_SHAPE shape)
        {
            var sql = @"INSERT INTO MD_SHAPE (ShapeName, Description, MinCarat, MaxCarat)
                        VALUES (@ShapeName, @Description, @MinCarat, @MaxCarat);
                        SELECT IDENT_CURRENT('MD_COLOR');";
            return await _conn.ExecuteScalarAsync<int>(sql, shape);
        }

        public async Task<bool> UpdateAsync(MD_SHAPE shape)
        {
            var sql = @"UPDATE MD_SHAPE
                        SET ShapeName = @ShapeName,
                            Description = @Description,
                            MinCarat = @MinCarat,
                            MaxCarat = @MaxCarat
                        WHERE ShapeId = @ShapeId";
            var rows = await _conn.ExecuteAsync(sql, shape);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM MD_SHAPE WHERE ShapeId = @Id";
            var rows = await _conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
