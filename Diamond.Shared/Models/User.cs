using System.Data;

namespace Diamond.Share.Models
{
    public class User
    {

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public string? ProfileImagePath { get; set; }
        public bool IsActive { get; set; }

    }
}
