using AutoMapper;
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
        private readonly IMapper mapper;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            this.notificationService = notificationService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] string userId)
        {
            var userNotifications = await notificationService.GetUserNotificationsAsync(userId);
            return Ok(mapper.Map<List<NotificationResponseDto>>(userNotifications));
        }

        [HttpGet("configure")]
        public async Task<IActionResult> GetUserNotificationSettings([FromQuery] string userId)
        {
            var userSettings = await notificationService.GetSettingsAsync(userId);
            return Ok(userSettings);
        }

        [HttpPut("configure")]
        public async Task<IActionResult> UpdateUserNotificationSetting([FromBody] NotificationSettingUpdateRequestDto notificationSettingUpdateRequestDto)
        {
            var updateSucceeded = await notificationService.UpdateSettingAsync(
                notificationSettingUpdateRequestDto.UserId,
                notificationSettingUpdateRequestDto.CategoryName,
                notificationSettingUpdateRequestDto.Enabled
            );

            if (!updateSucceeded)
                return NotFound(ValidationMessages.CategoryNotFound);

            return Ok(ValidationMessages.UpdateSuccess);
        }
    }
}
