using CrescentSchool.BLL.Enums;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/students")]
[Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Student) + "," + nameof(Roles.Instructor))]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetStudentsByIds([FromBody] List<Guid> studentIds, CancellationToken cancellationToken)
    {
        var result = await studentService.GetStudentsAsync(studentIds, cancellationToken);
        return Ok(result);
    }
    [HttpGet("GetStudentProfile")]
    public async Task<IActionResult> GetStudent([FromQuery] Guid studentId)
    {
        var result = await studentService.GetStudentByIdAsync(studentId);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
    [HttpPost("{id:guid}")]
    public async Task<IActionResult> CreateMonthlyReport([FromRoute] Guid id, [FromBody] MonthlyReportDto studentMonthlyReportDto)
    {
        await studentService.AddMonthlyReport(id, studentMonthlyReportDto);
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetMonthlyReports([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var result = await studentService.GetMonthlyReportsAsync(id, cancellationToken);
        return Ok(result);
    }
    [HttpGet("time-table/{id:guid}")]
    public async Task<IActionResult> GetTimeTable([FromRoute] Guid id, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var result = await studentService.GetUpcomingSessionsDtoAsync(id, startDate, endDate, cancellationToken);
        return Ok(result);
    }

}
