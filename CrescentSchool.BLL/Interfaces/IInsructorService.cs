using CrescentSchool.BLL.DTOs;

namespace CrescentSchool.BLL.Interfaces;

public interface IInsructorService
{
    Task<List<StudentDto>> GetInstructorStudents(Guid insrtructorId);
    Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId);
    Task<List<InstructorDto>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken);
}
