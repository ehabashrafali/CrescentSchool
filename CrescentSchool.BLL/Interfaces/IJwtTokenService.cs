using CrescentSchool.API.Entities;
using System.Security.Claims;

namespace CrescentSchool.BLL.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateToken(ApplicationUser user);
    ClaimsPrincipal? ValidateToken(string accessToken);
}
