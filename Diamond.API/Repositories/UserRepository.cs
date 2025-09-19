using Dapper;
using Diamond.API.Data;
using Diamond.API.Models;

namespace Diamond.API.Repositories
{
    public class UserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context) => _context = context;

        public async Task<int> CreateUserAsync(User user)
        {
            var sql = @"INSERT INTO Users (Username, Email, PasswordHash, PasswordSalt, Role, ProfilePicture, CreatedAt)
                        VALUES (@Username, @Email, @PasswordHash, @PasswordSalt, @Role, @ProfilePicture, @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleAsync<int>(sql, user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            using var conn = _context.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }
    }
}
