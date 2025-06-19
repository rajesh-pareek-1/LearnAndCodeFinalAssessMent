using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.DTOs.Admin;
using NewsSync.API.Models.DTO;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService service;

    public AdminController(IAdminService service)
    {
        this.service = service;
    }

    // POST /api/admin/category
    [HttpPost("category")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryCreateRequestDto dto)
    {
        await service.AddCategoryAsync(dto);
        return Ok("Category added.");
    }

    // GET /api/admin/server
    [HttpGet("server")]
    public async Task<IActionResult> GetServerStatus()
    {
        var status = await service.GetServerStatusAsync();
        return Ok(status);
    }

    // GET /api/admin/serverDetails
    [HttpGet("server/serverDetails")]
    public async Task<IActionResult> GetServerDetails()
    {
        var details = await service.GetServerDetailsAsync();
        return Ok(details);
    }

    // PUT /api/admin/server/{serverId}
    [HttpPut("server/{serverId}")]
    public async Task<IActionResult> UpdateServerApiKey(int serverId, [FromBody] ServerUpdateRequestDto dto)
    {
        await service.UpdateServerApiKeyAsync(serverId, dto.NewApiKey);
        return Ok("Server API key updated.");
    }
}
