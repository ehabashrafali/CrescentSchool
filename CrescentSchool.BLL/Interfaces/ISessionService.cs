using CrescentSchool.DAL.Dtos;
using CrescentSchool.Models.Enums;

namespace CrescentSchool.BLL.Interfaces;

public interface ISessionService
{
    Task<GetSessionsResult> GetSessionsAsync(int? pageNumber, int? pageNumber1, CancellationToken cancellationToken = default);
    Task<List<GetSessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<List<GetSessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default);
    Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken = default);
    Task CancelUpCommingSessionAsync(long meetingId, CancellationToken cancellationToken = default);
    Task<List<GetSessionDto>> GetSessionsByUserIdAndDate(Guid id, Role role, DateTimeOffset date, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken);
    Task<GetSessionDto?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> UpdateSessionAsync(Guid id, UpdateSessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<TimeTableSessionDto>> GetTimeTableSessionsDtoAsync(Guid id, Role role, CancellationToken cancellationToken = default);
}
