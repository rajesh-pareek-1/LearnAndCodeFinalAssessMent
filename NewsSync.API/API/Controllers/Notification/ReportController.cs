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
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> ReportArticle([FromBody] ReportDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.UserId) || dto.ArticleId <= 0)
                return BadRequest(ValidationMessages.UserIdRequired);

            try
            {
                await reportService.SubmitReportAsync(dto);
                return Ok(ValidationMessages.ArticleReported);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
