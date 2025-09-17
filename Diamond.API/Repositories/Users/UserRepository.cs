using Dapper;
using Diamond.API.Data;
using Diamond.Share.Models;

namespace Diamond.API.Repositories.Users
{
    public class UserRepository : IUserRepository
    {

        private readonly DapperContext _context;

        public UserRepository(DapperContext context) => _context = context;

        public async Task<MD_USER?> GetByEmailAsync(string email)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT * FROM MD_USER WHERE Email = @Email";
            return await conn.QueryFirstOrDefaultAsync<MD_USER>(sql, new { Email = email });
        }

        public async Task<MD_USER?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "SELECT * FROM MD_USER WHERE UserId = @Id";
            return await conn.QueryFirstOrDefaultAsync<MD_USER>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(MD_USER user)
        {
            using var conn = _context.CreateConnection();
            var sql = @"INSERT INTO MD_USER (UserName, Email, PasswordHash, Role, IsEmailConfirmed, 
                        ProfilePicturePath, CreatedAt) VALUES (@UserName, @Email, @Passwordhash, @Role, 
                        @IsEmailConfirmed, @ProfilePicturePath, SYSUTCDATETIME());
                        SELECT IDENT_CURRENT('MD_USER');";
            return await conn.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<bool> UpdateAsync(MD_USER user)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_USER SET 
                        UserName=@UserName, Email=@Email, Role=@Role, IsEmailConfirmed=@IsEmailConfirmed,
                        ProfilePicturePath=@ProfilePicturePath, UpdatedAt=SYSUTCDATETIME()
                        WHERE UserId=@UserId";

            var rows = await conn.ExecuteAsync(sql, user);
            return rows > 0;
        }

        public async Task<bool> UpdateProfilePictureAsync(int userId, string path)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_USER SET ProfilePicturePath=@Path, UpdatedAt=SYSUTCDATETIME() WHERE UserId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Path = path, Id = userId });
            return rows > 0;
        }

        public async Task<bool> SetPasswordResetTokenAsync(int userId, string token, DateTime expiry)
        {
            using var conn = _context.CreateConnection();
            var sql = @"UPDATE MD_USER SET PasswordResetToken=@Token, PasswordResetExpiry=@Expiry 
                        WHERE UserId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Token = token, Expiry = expiry, Id = userId });
            return rows > 0;
        }

        public async Task<MD_USER?> GetByResetTokenAsync(string token)
        {
            using var conn = _context.CreateConnection();
            var sql = @"SELECT * FROM MD_USER WHERE PasswordResetToken=@Token AND PasswordResetExpiry > SYSUTCDATETIME()";
            return await conn.QueryFirstOrDefaultAsync(sql, new { Token = token });
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newHash)
        {
            using var conn = _context.CreateConnection();
            var sql = "UPDATE MD_USER SET PasswordHash=@Hash, PasswordResetToken=NULL, PasswordResetExpiry=NULL, UpdatedAt=SYSUTCDATETIME() WHERE UserId=@Id";
            var rows = await conn.ExecuteAsync(sql, new { Hash = newHash, Id = userId });
            return rows > 0;
        }
    }
}
