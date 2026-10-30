using CrescentSchool.BLL.DTOs;
using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.BLL.Interfaces;

public interface IInstructorService
{
    Task<List<StudentDto>> GetInstructorStudents(Guid insrtructorId, CancellationToken cancellationToken = default);
    Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId, CancellationToken cancellationToken = default);
    Task<List<InstructorDto>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken = default);
    Task<Guid> CreateInstructorAsync(CreateInstructorDto createInstructorDto, CancellationToken cancellationToken = default);
    Task DeactivateInstructorAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> UpdateInstructorAsync(Guid id, UpdateInstructorDto updateInstructorDto, CancellationToken cancellationToken = default);
    Task DeleteInstructorAsync(Guid id, CancellationToken cancellationToken = default);
}
