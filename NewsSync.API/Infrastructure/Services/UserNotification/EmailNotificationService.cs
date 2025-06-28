using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace NewsSync.API.Application.Interfaces.Services
{
    public class EmailNotificationService : IUserNotificationService
    {
        private readonly IConfiguration _configuration;

        public EmailNotificationService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _configuration["Email:Smtp:Host"]
                ?? throw new InvalidOperationException("SMTP host configuration is missing.");

            var smtpPortString = _configuration["Email:Smtp:Port"];
            if (!int.TryParse(smtpPortString, out int smtpPort))
                throw new InvalidOperationException("SMTP port configuration is invalid or missing.");

            var smtpUser = _configuration["Email:Smtp:Username"]
                ?? throw new InvalidOperationException("SMTP username is missing.");
            var smtpPass = _configuration["Email:Smtp:Password"]
                ?? throw new InvalidOperationException("SMTP password is missing.");
            var fromEmail = _configuration["Email:From"]
                ?? throw new InvalidOperationException("From email address is missing.");

            using var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            using var mail = new MailMessage(fromEmail, toEmail, subject, body);

            await smtpClient.SendMailAsync(mail);
        }
    }
}
