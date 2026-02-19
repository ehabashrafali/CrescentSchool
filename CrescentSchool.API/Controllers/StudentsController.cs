using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/students")]
//[Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Student) + "," + nameof(Roles.Instructor))]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetStudentsByIds([FromBody] List<Guid> studentIds, CancellationToken cancellationToken)
    {
        var result = await studentService.GetStudentsAsync(studentIds, cancellationToken);
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

    [HttpGet("monthly-report/{id:guid}")]
    public async Task<IActionResult> GetCurrentMonthReport([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await studentService.GetCurrentMonthReport(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("deactivate/{id:guid}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await studentService.DeactivateStudentAsync(id, cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, [FromBody] UpdateStudentDto updateStudentDto, CancellationToken cancellationToken)
    {
        await studentService.UpdateStudentAsync(id, updateStudentDto, cancellationToken);
        return Ok();
    }
    [HttpGet("GetStudent/{id:guid}")]
    public async Task<IActionResult> GetStudentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await studentService.GetStudentByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost("create-student")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto, CancellationToken cancellationToken)
    {
        var result = await studentService.CreateStudentAsync(createStudentDto, cancellationToken);
        return Ok(result);
    }
}
