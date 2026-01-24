using CrescentSchool.Models;

namespace CrescentSchool.DAL.Repositories
{
    public interface IInstructorsRepository
    {
        Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds);
        Task<Instructor?> GetByIdAsync(Guid instructorId);
        Task<List<Student>> GetInstuctorStudents(Guid instructorId);
    }
}
