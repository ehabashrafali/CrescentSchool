using CrescentSchool.Models;
using CrescentSchool.Models.Dtos;

namespace CrescentSchool.DAL.Repositories;

public interface IStudentsRepository
{
    public Task<List<Student>> GetStudentsByIdsAsync(List<Guid> studentIds, CancellationToken cancellation = default);
    public Task<Student?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    public Task AddMonthlyReport(Guid studentId, MonthlyReportDto studentMonthlyReportDto);
    public Task<List<StudentMonthlyReport>> GetMonthlyReports(Guid id, CancellationToken cancellation);
}
