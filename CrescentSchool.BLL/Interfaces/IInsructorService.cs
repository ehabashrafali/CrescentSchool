using CrescentSchool.BLL.DTOs;
using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.BLL.Interfaces;

public interface IInsructorService
{
    Task<List<StudentDto>> GetInstructorStudents(Guid insrtructorId);
    Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId);
    Task<List<InstructorDto>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken);
    Task<Guid> CreateInstructorAsync(CreateInstructorDto createInstructorDto, CancellationToken cancellationToken);
    Task DeactivateInstructorAsync(Guid id, CancellationToken cancellationToken);
}
