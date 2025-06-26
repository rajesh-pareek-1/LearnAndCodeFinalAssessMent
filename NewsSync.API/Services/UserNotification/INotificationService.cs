namespace NewsSync.API.Services.Notification
{
    public interface INotificationService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
