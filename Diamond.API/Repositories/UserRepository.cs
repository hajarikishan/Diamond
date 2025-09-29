using Dapper;
using Diamond.Share.Models;
using System.Data;

namespace Diamond.API.Repositories
{
    public class UserRepository
    {

        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<User?> GetByUserName(string userName)
        {
            var sql = "SELECT * FROM Users u INNER JOIN Roles r ON u.RoleId = r.RoleId WHERE u.UserName=@UserName";
            var result = await _db.QueryAsync<User, Role, User>(
                sql,
                (user, role) => { user.Role = role; return user; },
                new { UserName = userName },
                splitOn: "RoleId"
            );
            return result.FirstOrDefault();
        }

        public async Task<int> CreateUser(User user)
        {
            var sql = "INSERT INTO Users (UserName, PasswordHash, FullName, RoleId) VALUES (@UserName, @PasswordHash, @FullName, @RoleId)";
            return await _db.ExecuteAsync(sql, user);
        }

        public async Task<User?> GetById(int id)
        {
            var sql = "SELECT u.*, r.RoleId, r.RoleName FROM Users u INNER JOIN Roles r ON u.RoleId = r.RoleId WHERE u.UserId = @Id";
            var result = await _db.QueryAsync<User, Role, User>(
                sql,
                (user, role) => { user.Role = role; return user; },
                new { Id = id },
                splitOn: "RoleId"
            );
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<RoleCount>> GetUserCountsByRole()
        {
            var sql = @"SELECT r.RoleName AS RoleName, COUNT(*) AS Count
                        FROM Users u INNER JOIN Roles r ON u.RoleId = r.RoleId
                        GROUP BY r.RoleName";
            return (await _db.QueryAsync<RoleCount>(sql)).ToList();
        }

        public async Task<int> UpdateProfileImage(int userId, string? imagePath)
        {
            var sql = "UPDATE Users SET ProfileImagePath = @Path WHERE UserId = @Id";
            return await _db.ExecuteAsync(sql, new { Path = imagePath, Id = userId });
        }

        public async Task<int> ChangePassword(int userId, string newPasswordHash)
        {
            var sql = "UPDATE Users SET PasswordHash = @Hash WHERE UserId = @Id";
            return await _db.ExecuteAsync(sql, new { Hash = newPasswordHash, Id = userId });
        }

        public async Task<int> CreatePasswordResetRequest(int userId, string resetToken, string otpHash, DateTime expiresAt)
        {
            var sql = @"INSERT INTO PasswordResetRequest (UserId, ResetToken, OtpHash, ExpiresAt)
                        VALUES (@UserId, @ResetToken, @OtpHash, @ExpiresAt);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            return await _db.QuerySingleAsync<int>(sql, new { userId = userId, ResetToken = resetToken, OtpHash = otpHash, ExpiresAt = expiresAt });
        }

        public async Task<PasswordResetRequest?> GetPasswordResetRequestAsync(int userId, string? resetToken = null)
        {
            var sql = @"SELECT TOP 1 * FROM PasswordResetRequest
                        WHERE UserId = @UserId AND IsUsed = 0 AND ExpiresAt > GETUTCDATE()
                        ORDER BY CreatedAt DESC";

            if (!string.IsNullOrEmpty(resetToken))
            {
                sql = @"SELECT TOP 1 * FROM PasswordResetRequest 
                        WHERE UserId = @UserId AND ResetToken = @ResetToken AND IsUsed = 0 AND ExpiresAt > GETUTCDATE()
                        ORDER BY CreatedAt DESC";

                return (await _db.QueryAsync<PasswordResetRequest>(sql, new { UserId = userId, ResetToken = resetToken })).FirstOrDefault();
            }
            return (await _db.QueryAsync<PasswordResetRequest>(sql, new { userId = userId })).FirstOrDefault();
        }

        public async Task<int> MarkResetRequestUsed(int requestId)
        {
            var sql = "UPDATE PasswordResetRequest SET IsUsed = 1 WHERE RequestId = @RequestId";
            return await _db.ExecuteAsync(sql, new { RequestId = requestId });
        }

        public async Task<List<User>> GetAllUsers()
        {
            var sql = "SELECT u.*, r.RoleId, r.RoleName FROM Users u INNER JOIN Roles r ON u.RoleId=r.RoleId";
            var list = await _db.QueryAsync<User, Role, User>(sql, (u, r) => { u.Role = r; return u; }, splitOn: "RoleId");
            return list.ToList();
        }

        public async Task<int> UpdateUserRoleAndActive(int userId, int roleId, bool isActive)
        {
            var sql = "UPDATE Users SET RoleId = @RoleId, IsActive = @IsActive WHERE UserId = @UserId";
            return await _db.ExecuteAsync(sql, new { RoleId = roleId, IsActive = isActive, UserId = userId });
        }

    }
}
