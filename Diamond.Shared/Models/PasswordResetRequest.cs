namespace Diamond.Share.Models
{
    public class PasswordResetRequest
    {

        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string? ResetToken { get; set; }
        public string? OtpHash { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

    }
}
