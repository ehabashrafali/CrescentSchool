using CrescentSchool.Core.Exceptions;
using CrescentSchool.Core.Helpers;
using CrescentSchool.Core.Models;

namespace CrescentSchool.Core.Extensions;

public static class ErrorResponseExtensions
{
    public static ErrorResponse ToErrorResponse(this Exception exception)
        => exception switch
        {
            UnauthorizedAccessException => ErrorResponseHelper.GetUnauthorizedResponse(),
            UnauthorizedException => ErrorResponseHelper.GetUnauthorizedResponse(),
            ForbiddenAccessException => ErrorResponseHelper.GetForbiddenResponse(),
            NotFoundException => ErrorResponseHelper.GetNotFoundResponse(exception.Message),
            ValidationException => ErrorResponseHelper.GetBadRequestResponse(
                GetErrors(GetValidationException(exception))),
            ConflictException =>
                ErrorResponseHelper.GetConflictResponse(GetConflictException(exception)?.LatestVersion),
            _ => ErrorResponseHelper.GetInternalServerErrorResponse()
        };

    private static ValidationException? GetValidationException(Exception exception)
    {
        if (exception is ValidationException validationException) return validationException;
        return null;
    }

    private static ConflictException? GetConflictException(Exception exception)
    {
        if (exception is ConflictException conflictException) return conflictException;
        return null;
    }

    private static IReadOnlyDictionary<string, ValidationError[]> GetErrors(ValidationException? exception)
        => exception?.ErrorsDictionary ?? new Dictionary<string, ValidationError[]>();
}
