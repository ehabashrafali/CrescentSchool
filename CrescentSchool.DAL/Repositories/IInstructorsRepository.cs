using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
namespace CrescentSchool.DAL.Repositories;

public interface IInstructorsRepository
{
    Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken = default);
    Task<Instructor?> GetByIdAsync(Guid instructorId);
    Task<List<Student>> GetInstuctorStudents(Guid instructorId);
    Task<Guid> CreateInstructorAsync(CreateInstructorDto createInstructorDto, CancellationToken cancellationToken);
    Task DeactivateInstructor(Guid id, CancellationToken cancellationToken);
}
