using CrescentSchool.Models;

namespace CrescentSchool.DAL.Repositories
{
    public interface IInstructorsRepository
    {
        Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken = default);
        Task<Instructor?> GetByIdAsync(Guid instructorId);
        Task<List<Student>> GetInstuctorStudents(Guid instructorId);
    }
}
