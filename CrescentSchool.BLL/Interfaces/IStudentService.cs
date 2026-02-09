using CrescentSchool.BLL.DTOs;
using CrescentSchool.Models;
using CrescentSchool.Models.Dtos;

namespace CrescentSchool.BLL.Interfaces;

public interface IStudentService
{
    Task<StudentDto?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<List<StudentDto>> GetStudentsAsync(List<Guid> studentIds, CancellationToken cancellationToken = default);
    Task AddMonthlyReport(Guid studentId, MonthlyReportDto studentMonthlyReportDto);
    Task<List<MonthlyReportDto>> GetMonthlyReportsAsync(Guid id, CancellationToken cancellation = default);
    Task<List<UpcomingSessionDto>> GetUpcomingSessionsDtoAsync(Guid id, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    Task<MonthlyReportDto?> GetCurrentMonthReport(Guid id, CancellationToken cancellationToken);
}
