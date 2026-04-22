using CrescentSchool.API.Enums;
using CrescentSchool.API.Extensions;
using CrescentSchool.API.Models;

namespace CrescentSchool.API.Helpers;

public static class ErrorResponseHelper
{
    public static ErrorResponse GetUnauthorizedResponse() =>
        new("Unauthorized", ErrorCode.Unauthorized, "Unauthorized access");

    public static ErrorResponse GetForbiddenResponse() =>
        new("Forbidden", ErrorCode.Forbidden, "Forbidden access");

    public static ErrorResponse GetNotFoundResponse(string? message = default) =>
        new("Not Found", ErrorCode.NotFound, message.IsNullOrEmpty() ? "Resource not found" : message!);

    public static ErrorResponse GetBadRequestResponse(string message) =>
        new("Validation Errors", ErrorCode.ModelValidation, message);

    public static ErrorResponse GetBadRequestResponse(IReadOnlyDictionary<string, ValidationError[]>? details) =>
        new("Validation Errors", ErrorCode.ModelValidation, "One or more validation errors occurred.",
            details: details);

    public static ErrorResponse GetInternalServerErrorResponse() =>
        new("Internal Server Error", ErrorCode.InternalServerError, "Internal Server Error");

    public static ErrorResponse GetConflictResponse(Guid? latestVersion) =>
        new("Conflict", ErrorCode.Conflict, "Conflict",
            latestVersion: latestVersion);
}
