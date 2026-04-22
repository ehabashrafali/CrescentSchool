using CrescentSchool.API.ExceptionHandling;

namespace CrescentSchool.API.Exceptions;

public class ForbiddenAccessException : NonRetryableException
{
    public ForbiddenAccessException()
        : base()
    {
    }
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
