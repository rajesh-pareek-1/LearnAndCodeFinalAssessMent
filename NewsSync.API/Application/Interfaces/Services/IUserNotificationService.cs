namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IUserNotificationService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
