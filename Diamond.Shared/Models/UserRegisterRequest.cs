namespace Diamond.Share.Models
{
    public class UserRegisterRequest
    {

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }

    }
}
