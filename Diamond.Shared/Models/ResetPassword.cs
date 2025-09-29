namespace Diamond.Share.Models
{
    public class ResetPassword
    {

        public string UserName { get; set; } = "";
        public string ResetToken { get; set; } = ""; // token from VerifyOtp response
        public string NewPassword { get; set; } = "";

    }
}
