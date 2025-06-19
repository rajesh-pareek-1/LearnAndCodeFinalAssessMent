using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        // GET: /api/notification?userId=xxx
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");

            var notifications = await notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        // GET: /api/notification/configure?userId=xxx
        [HttpGet("configure")]
        public async Task<IActionResult> GetNotificationSettings([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");

            var settings = await notificationService.GetSettingsAsync(userId);
            return Ok(settings);
        }

        // PUT: /api/notification/configure
        [HttpPut("configure")]
        public async Task<IActionResult> UpdateNotificationSetting([FromBody] NotificationSettingUpdateRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest("UserId and CategoryName are required.");

            var success = await notificationService.UpdateSettingAsync(request.UserId, request.CategoryName, request.Enabled);

            if (!success)
                return NotFound("Category not found.");

            return Ok("Updated successfully.");
        }
    }
}
