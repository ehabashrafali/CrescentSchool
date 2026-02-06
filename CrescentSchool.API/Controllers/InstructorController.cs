using CrescentSchool.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/instructors")]
//[Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Instructor))]
public class InstructorController(IInsructorService instructorService) : ControllerBase
{
    [HttpGet("GetInstructorStudents")]
    public async Task<IActionResult> GetInstructorStudents([FromQuery] Guid instructorId)
    {
        var result = await instructorService.GetInstructorStudents(instructorId);
        return Ok(result);
    }

    [HttpGet("GetInstructorProfile")]
    public async Task<IActionResult> GetInstructorInfo([FromQuery] Guid instructorId)
    {
        var result = await instructorService.GetInstructorByIdAsync(instructorId);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> GetInstructorsByIds([FromBody] List<Guid> instructorIds, CancellationToken cancellationToken)
    {
        var result = await instructorService.GetInstructorsAsync(instructorIds, cancellationToken);
        return Ok(result);
    }

}
