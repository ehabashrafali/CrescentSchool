using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
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
    [HttpPost("create-instructor")]
    public async Task<IActionResult> CreateInstructor([FromBody] CreateInstructorDto createInstructorDto, CancellationToken cancellationToken)
    {
        var result = await instructorService.CreateInstructorAsync(createInstructorDto, cancellationToken);
        return Ok(result);
    }
    [HttpPut("deactivate/{id:guid}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await instructorService.DeactivateInstructorAsync(id, cancellationToken);
        return Ok();
    }

}
