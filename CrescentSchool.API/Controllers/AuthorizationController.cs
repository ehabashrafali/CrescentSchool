using CrescentSchool.API.Entities;
using CrescentSchool.API.Models;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthorizationController(UserManager<ApplicationUser> _userManager, IJwtTokenService _jwtService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        var token = await _jwtService.CreateToken(user);
        return Ok(new { token });
    }

    [HttpPost("sign-in-with-token")]
    public async Task<IActionResult> SignInWithToken([FromBody] SignInWithTokenRequest request)
    {
        var principal = _jwtService.ValidateToken(request.AccessToken);
        if (principal == null)
            return Unauthorized();

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim))
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        var newToken = _jwtService.CreateToken(user);

        return Ok(new
        {
            token = newToken
        });
    }
}