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
    [Authorize(Roles = RoleNames.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost("category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateRequestDto categoryCreateRequestDto)
        {
            await adminService.AddCategoryAsync(categoryCreateRequestDto);
            return Ok(ValidationMessages.CategoryAdded);
        }

        [HttpGet("server")]
        public async Task<IActionResult> GetServerStatus()
        {
            var status = await adminService.GetServerStatusAsync();
            return Ok(status);
        }

        [HttpGet("server/details")]
        public async Task<IActionResult> GetServerDetails()
        {
            var details = await adminService.GetServerDetailsAsync();
            return Ok(details);
        }

        [HttpPut("server/{serverId}")]
        public async Task<IActionResult> UpdateServerApiKey(int serverId, [FromBody] ServerUpdateRequestDto dto)
        {
            await adminService.UpdateServerApiKeyAsync(serverId, dto.NewApiKey);
            return Ok(ValidationMessages.ServerApiKeyUpdated);
        }

        [HttpPut("article/{articleId}/block")]
        public async Task<IActionResult> BlockArticle(int articleId, [FromQuery] bool block)
        {
            await adminService.BlockArticleAsync(articleId, block);
            return Ok(block ? ValidationMessages.ArticleBlocked : ValidationMessages.ArticleUnblocked);
        }
    }
}
