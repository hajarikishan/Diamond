namespace Diamond.Share.Models
{
    public class VerifyOtp
    {

        public string UserName { get; set; } = "";   // same username used in Forgot step
        public string Otp { get; set; } = "";

    }
}
