using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Infrastructure.Configurations;

namespace NewsSync.API.Application.Services
{
    public class EmailNotificationService : IUserNotificationService
    {
        private readonly SmtpSettings smtpSettings;
        private readonly ILogger<EmailNotificationService> logger;

        public EmailNotificationService(IOptions<SmtpSettings> smtpOptions, ILogger<EmailNotificationService> logger)
        {
            this.smtpSettings = smtpOptions?.Value ?? throw new ArgumentNullException(nameof(smtpOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                logger.LogWarning("Email not sent. Invalid input.");
                throw new ArgumentException(ValidationMessages.InvalidEmailInput);
            }

            try
            {
                using var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
                };

                using var mail = new MailMessage(smtpSettings.From, toEmail, subject, body);
                await smtpClient.SendMailAsync(mail);

                logger.LogInformation("Email sent to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                throw new ApplicationException(ValidationMessages.EmailSendFailed, ex);
            }
        }
    }
}
