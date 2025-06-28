using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<bool> SubmitReportAsync(ReportDto reportDto);
    }
}
