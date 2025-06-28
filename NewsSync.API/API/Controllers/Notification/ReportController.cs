using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSync.API.Models.Contants;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.User)]
    public class ReportController : ControllerBase
    {

        private readonly IMapper _mapper;

        public ILogger<AdminController> _logger { get; }
        public NewsSyncNewsDbContext _newsDbContext { get; }

        public ReportController(ILogger<AdminController> logger, IMapper mapper, NewsSyncNewsDbContext _newsDbContext)
        {
            _logger = logger;
            this._mapper = mapper;
            this._newsDbContext = _newsDbContext;
        }

        [HttpPost("api/report")]
        public async Task<IActionResult> ReportArticle([FromBody] ReportDto dto)
        {
            var exists = await _newsDbContext.ArticleReports
                .AnyAsync(r => r.ArticleId == dto.ArticleId && r.ReportedByUserId == dto.UserId);

            if (exists)
                return BadRequest("You’ve already reported this article.");

            var report = new ArticleReport
            {
                ArticleId = dto.ArticleId,
                ReportedByUserId = dto.UserId,
                Reason = dto.Reason
            };

            _newsDbContext.ArticleReports.Add(report);
            await _newsDbContext.SaveChangesAsync();

            var totalReports = await _newsDbContext.ArticleReports
                .CountAsync(r => r.ArticleId == dto.ArticleId);

            if (totalReports >= 3)
            {
                var article = await _newsDbContext.Articles.FindAsync(dto.ArticleId);
                if (article is not null && !article.IsBlocked)
                {
                    article.IsBlocked = true;
                    await _newsDbContext.SaveChangesAsync();
                }
            }

            return Ok("Article reported.");
        }

    }
}
