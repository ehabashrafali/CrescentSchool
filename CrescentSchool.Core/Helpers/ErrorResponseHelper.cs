using CrescentSchool.Core.Enums;
using CrescentSchool.Core.Extensions;
using CrescentSchool.Core.Models;

namespace CrescentSchool.Core.Helpers;

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

    public static ErrorResponse GetInternalServerErrorResponse(string message) =>
       new("Internal Server Error", ErrorCode.InternalServerError, message);


    public static ErrorResponse GetConflictResponse(Guid? latestVersion) =>
        new("Conflict", ErrorCode.Conflict, "Conflict",
            latestVersion: latestVersion);
}
