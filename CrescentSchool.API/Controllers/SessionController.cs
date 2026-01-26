using CrescentSchool.BLL.Enums;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/sessions")]

public class SessionController(ISessionService sessionService) : ControllerBase
{
    [HttpGet("student/{id:guid}")]
    public async Task<IActionResult> GetSessionsByStudentId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionsByStudentIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("instructor/{id:guid}")]
    public async Task<IActionResult> GetSessionsByInstructorId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionsByStudentIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("create-session")]
    public async Task<IActionResult> CreateSession(SessionDto sessionDto, CancellationToken cancellationToken)
    {
        var result = await sessionService.CreateSession(sessionDto, cancellationToken);

        if (result is null)
            return NotFound("Not Active Student");

        return Ok(result);
    }

    [HttpGet("current-month/{id:guid}")]
    public async Task<IActionResult> GetOfCurrentMonthAndYear([FromRoute] Guid id, [FromQuery] Roles role, [FromQuery] DateTimeOffset date, CancellationToken cancellationToken)
    {
        var result = await sessionService.GetSessionsOfCurrentMonthAndYear(id, role, date, cancellationToken);
        return Ok(result);
    }

}
