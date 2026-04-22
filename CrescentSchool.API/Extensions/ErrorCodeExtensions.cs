using CrescentSchool.API.Enums;
using Microsoft.AspNetCore.Http;

namespace CrescentSchool.API.Extensions;

public static class ErrorCodeExtensions
{
    public static int ToStatusCode(this ErrorCode errorCode)
    {
        return errorCode switch
        {
            ErrorCode.ModelValidation => StatusCodes.Status400BadRequest,
            ErrorCode.BusinessValidation => StatusCodes.Status400BadRequest,
            ErrorCode.DomainValidation => StatusCodes.Status400BadRequest,
            ErrorCode.NotFound => StatusCodes.Status404NotFound,
            ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCode.InternalServerError => StatusCodes.Status500InternalServerError,
            ErrorCode.Conflict => StatusCodes.Status409Conflict,
            ErrorCode.Warning => StatusCodes.Status400BadRequest,
            _ => throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, null)
        };
    }
}
