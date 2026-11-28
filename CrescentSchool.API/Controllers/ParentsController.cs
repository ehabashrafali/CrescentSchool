using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/parents")]
public class ParentsController(IParentService parentService) : ControllerBase
{
    [HttpPost("create-parent")]
    public async Task<IActionResult> CreateParent([FromBody] CreateParentDto createParentDto, CancellationToken cancellationToken)
    {
        var result = await parentService.CreateParentAsync(createParentDto, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetParents(CancellationToken cancellationToken)
    {
        var result = await parentService.GetParentsAsync(cancellationToken);
        return Ok(result);
    }
}
