using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Common.Messages;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.User)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(ValidationMessages.UserIdRequired);

            var userNotifications = await notificationService.GetUserNotificationsAsync(userId);
            return Ok(userNotifications);
        }

        [HttpGet("configure")]
        public async Task<IActionResult> GetUserNotificationSettings([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(ValidationMessages.UserIdRequired);

            var userSettings = await notificationService.GetSettingsAsync(userId);
            return Ok(userSettings);
        }

        [HttpPut("configure")]
        public async Task<IActionResult> UpdateUserNotificationSetting([FromBody] NotificationSettingUpdateRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest(ValidationMessages.UserIdAndCategoryRequired);

            var updateSucceeded = await notificationService.UpdateSettingAsync(
                request.UserId,
                request.CategoryName,
                request.Enabled
            );

            if (!updateSucceeded)
                return NotFound(ValidationMessages.CategoryNotFound);

            return Ok(ValidationMessages.UpdateSuccess);
        }
    }
}
