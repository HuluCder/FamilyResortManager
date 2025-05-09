using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace FamilyResortManager.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = _config.GetSection("SmtpSettings");
            using var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]))
            {
                EnableSsl = bool.Parse(smtp["EnableSsl"]),
                Credentials = new NetworkCredential(smtp["User"], smtp["Password"])
            };

            var message = new MailMessage(smtp["User"], to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
        }
    }
}