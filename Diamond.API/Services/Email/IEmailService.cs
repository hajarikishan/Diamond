namespace Diamond.API.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlbody);
    }
}
