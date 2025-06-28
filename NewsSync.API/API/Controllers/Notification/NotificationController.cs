using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models.Contants;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Services;

namespace NewsSync.API.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.User)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService _notificationService)
        {
            this._notificationService = _notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("configure")]
        public async Task<IActionResult> GetNotificationSettings([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");

            var settings = await _notificationService.GetSettingsAsync(userId);
            return Ok(settings);
        }

        [HttpPut("configure")]
        public async Task<IActionResult> UpdateNotificationSetting([FromBody] NotificationSettingUpdateRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest("UserId and CategoryName are required.");

            var success = await _notificationService.UpdateSettingAsync(request.UserId, request.CategoryName, request.Enabled);

            if (!success)
                return NotFound("Category not found.");

            return Ok("Updated successfully.");
        }
    }
}
