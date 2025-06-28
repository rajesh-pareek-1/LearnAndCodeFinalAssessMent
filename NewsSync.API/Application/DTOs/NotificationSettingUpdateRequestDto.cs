namespace NewsSync.API.Application.DTOs
{
    public class NotificationSettingUpdateRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
