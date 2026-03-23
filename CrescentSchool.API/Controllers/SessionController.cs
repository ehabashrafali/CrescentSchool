using CrescentSchool.BLL.Enums;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
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
    public async Task<IActionResult> GetSessionsByIdAndDate([FromRoute] Guid id, [FromQuery] Roles role, [FromQuery] DateTimeOffset date, CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionsByIdAndDate(id, role, date, cancellationToken);
        return Ok(result);
    }
    [HttpGet("search")]
    public async Task<IActionResult> GetSessions(CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpDelete("delete-session/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sessionService.DeleteSessionAsync(id, cancellationToken);
        return Ok();
    }

    [HttpGet("get-sessions-by-id/{studentId:guid}")]
    public async Task<IActionResult> GetSessionsById([FromRoute] Guid studentId, [FromQuery] Roles role, CancellationToken cancellationToken)
    {
        var result = new List<GetSessionDto>();
        if (role is Roles.Student)
            result = await sessionService.GetSessionsByStudentIdAsync(studentId, cancellationToken);
        else if (role is Roles.Instructor)
            result = await sessionService.GetSessionsByInstructorIdAsync(studentId, cancellationToken);
        return Ok(result);
    }

}
