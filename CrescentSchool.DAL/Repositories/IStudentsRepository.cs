using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;

namespace CrescentSchool.DAL.Repositories;

public interface IStudentsRepository
{
    public Task<List<Student>> GetStudentsByIdsAsync(List<Guid> studentIds, CancellationToken cancellation = default);
    public Task<Student?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    public Task AddMonthlyReport(Guid studentId, MonthlyReportDto studentMonthlyReportDto);
    public Task<List<StudentMonthlyReport>> GetMonthlyReports(Guid id, CancellationToken cancellation);
    Task<StudentMonthlyReport?> GetCurrentMonthlyReport(Guid id, CancellationToken cancellationToken);
    Task DeactivateStudent(Guid id, CancellationToken cancellationToken);
    Task UpdateStudent(Student student, CancellationToken cancellationToken);
    Task<Guid> CreateStudentAsync(CreateStudentDto createStudentDto, CancellationToken cancellationToken);
}
