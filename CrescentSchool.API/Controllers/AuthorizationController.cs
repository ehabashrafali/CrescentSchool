using CrescentSchool.API.Models;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.Core.Exceptions;
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
            throw new UnauthorizedException("Invalid credentials");

        var token = await _jwtService.CreateToken(user);
        return Ok(new { token });
    }

    [HttpPost("sign-in-with-token")]
    public async Task<IActionResult> SignInWithToken([FromBody] SignInWithTokenRequest request)
    {
        var principal = _jwtService.ValidateToken(request.AccessToken);
        if (principal == null)
            throw new UnauthorizedException("Invalid token");

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim))
            throw new UnauthorizedException("Invalid token: missing user identifier");

        var user = await _userManager.FindByIdAsync(userIdClaim);
        if (user is null)
            throw new UnauthorizedException("User not found");

        var newToken = _jwtService.CreateToken(user);

        return Ok(new
        {
            token = newToken
        });
    }
}