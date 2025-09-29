
using MailKit.Net.Smtp;
using MimeKit;

namespace Diamond.API.Services.Email
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) => _config = config;

        public async Task SendAsync(string to, string subject, string htmlbody)
        {
            var emailCfg = _config.GetSection("Email");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("kishan", "kishanhajari2422@gmail.com"));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var body = new BodyBuilder { HtmlBody = htmlbody };
            message.Body = body.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(emailCfg["SmtpHost"], int.Parse(emailCfg["SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailCfg["Username"], emailCfg["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
