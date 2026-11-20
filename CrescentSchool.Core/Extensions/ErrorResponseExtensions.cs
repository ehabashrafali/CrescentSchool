using CrescentSchool.Core.Exceptions;
using CrescentSchool.Core.Helpers;
using CrescentSchool.Core.Models;
using CrescentSchool.Host.Extensions;

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
            BusinessValidationException => ErrorResponseHelper.GetBadRequestResponse(exception.Message),
            ValidationException validationException when validationException.ErrorsDictionary.Count == 0 =>
                ErrorResponseHelper.GetBadRequestResponse(validationException.Message),
            ValidationException validationException => ErrorResponseHelper.GetBadRequestResponse(
                validationException.ErrorsDictionary),
            ConflictException =>
                ErrorResponseHelper.GetConflictResponse(GetConflictException(exception)?.LatestVersion),
            _ => ErrorResponseHelper.GetInternalServerErrorResponse(exception.Message)
        };

    private static ConflictException? GetConflictException(Exception exception)
    {
        if (exception is ConflictException conflictException) return conflictException;
        return null;
    }

}
