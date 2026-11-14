using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/sessions")]

public class SessionController(ISessionService sessionService) : ControllerBase
{
    [HttpPost("create-session")]
    public async Task<IActionResult> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken)
    {
        var result = await sessionService.CreateSession(sessionDto, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSessionsByUserIdAndDate([FromRoute] Guid id, [FromQuery] Role role, [FromQuery] DateTimeOffset date, CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionsByUserIdAndDate(id, role, date, cancellationToken);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetSessions([FromQuery] int? pageNumber,
                                                 [FromQuery] int? pageSize,
                                                 CancellationToken cancellationToken = default)
    {
        var result = await sessionService.GetSessionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("delete-session/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sessionService.DeleteSessionAsync(id, cancellationToken);
        return Ok();
    }

    [HttpGet("get-sessions-by-user-id/{id:guid}")]
    public async Task<IActionResult> GetSessionsByUserId([FromRoute] Guid id, [FromQuery] Role role, CancellationToken cancellationToken)
    {
        var result = new List<GetSessionDto>();
        if (role is Role.Student)
            result = await sessionService.GetSessionsByStudentIdAsync(id, cancellationToken);
        else if (role is Role.Instructor)
            result = await sessionService.GetSessionsByInstructorIdAsync(id, cancellationToken);
        return Ok(result);
    }
    [HttpGet("get-session-by-id/{id:guid}")]
    public async Task<IActionResult> GetSessionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionByIdAsync(id, cancellationToken);
        return Ok(result);
    }
    [HttpPut("update-session/{id:guid}")]
    public async Task<IActionResult> UpdateSession([FromRoute] Guid id, [FromBody] UpdateSessionDto sessionDto, CancellationToken cancellationToken)
    {
        var session = await sessionService.UpdateSessionAsync(id, sessionDto, cancellationToken);
        return Ok(session);
    }

    [HttpGet("time-table/{id:guid}")]
    public async Task<IActionResult> GetTimeTable([FromRoute] Guid id, [FromQuery] Role role, CancellationToken cancellationToken = default)
    {
        var result = await sessionService.GetTimeTableSessionsDtoAsync(id, role, cancellationToken);
        return Ok(result);
    }
    [HttpDelete("cancel-upcomming-session/{meetingId:long}")]
    public async Task<IActionResult> CancelUpCommingSession([FromRoute] long meetingId, CancellationToken cancellationToken)
    {
        await sessionService.CancelUpCommingSessionAsync(meetingId, cancellationToken);
        return Ok();
    }
}
