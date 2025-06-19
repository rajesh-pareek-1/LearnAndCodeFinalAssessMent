using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace NewsSync.API.Services.Notification
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public EmailNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _configuration["Email:Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Email:Smtp:Port"]);
            var smtpUser = _configuration["Email:Smtp:Username"];
            var smtpPass = _configuration["Email:Smtp:Password"];
            var fromEmail = _configuration["Email:From"];

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            var mail = new MailMessage(fromEmail, toEmail, subject, body);

            await smtp.SendMailAsync(mail);
        }
    }
}
