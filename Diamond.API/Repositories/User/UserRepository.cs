using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models.Auth;

namespace Diamond.API.Repositories.User
{
    public class UserRepository
    {

        private readonly DapperContext _context;

        public UserRepository(DapperContext context) => _context = context;

        public async Task<UserProfileDto> GetByEmailAsync(string email)
        {
            var sql = "SELECT TOP 1 * FROM Users WHERE Email = @Email";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserProfileDto>(sql, new { Email = email });
        }

        public async Task<int> RegisterAsync(RegisterRequest request, string passwordHash)
        {
            var sql = @"INSERT INTO Users (Username, Email, PasswordHash, Role) 
                        VALUES (@Username, @Email, @PasswordHash, 'User')";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(sql, new
            {
                request.Username,
                request.Email,
                PasswordHash = passwordHash
            });
        }

        public async Task<string> GetPasswordHashAsync(string email)
        {
            var sql = "SELECT PasswordHash FROM Users WHERE Email = @Email";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { Email = email });
        }

    }
}
