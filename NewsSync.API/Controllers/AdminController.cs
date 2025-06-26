using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models.DTO;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public ILogger<AdminController> _logger { get; }

        public AdminController(IAdminService service, ILogger<AdminController> logger, IMapper mapper)
        {
            this._adminService = service;
            _logger = logger;
            this._mapper = mapper;
        }

        [HttpPost("category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateRequestDto dto)
        {
            await _adminService.AddCategoryAsync(dto);
            return Ok("Category added.");
        }

        [HttpPost("fakesmln")]
        public async Task<IActionResult> fakeSmln([FromBody] CategoryCreateRequestDto dto)
        {
            //fake smln
            _logger.LogInformation($"get all call finished ");
            throw new Exception("Simulated server crash");
        }

        [HttpGet("server")]
        public async Task<IActionResult> GetServerStatus()
        {
            var status = await _adminService.GetServerStatusAsync();
            return Ok(status);
        }

        [HttpGet("server/serverDetails")]
        public async Task<IActionResult> GetServerDetails()
        {
            var details = await _adminService.GetServerDetailsAsync();
            return Ok(details);
        }

        [HttpPut("server/{serverId}")]
        public async Task<IActionResult> UpdateServerApiKey(int serverId, [FromBody] ServerUpdateRequestDto dto)
        {
            await _adminService.UpdateServerApiKeyAsync(serverId, dto.NewApiKey);
            return Ok("Server API key updated.");
        }

        [HttpPut("article/{articleId}/block")]
        public async Task<IActionResult> BlockArticle(int articleId, [FromQuery] bool block)
        {
            await _adminService.BlockArticleAsync(articleId, block);
            return Ok($"Article has been {(block ? "blocked" : "unblocked")}.");
        }
    }
}
