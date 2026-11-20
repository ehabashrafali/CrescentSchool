using CrescentSchool.Core.ExceptionHandling;

namespace CrescentSchool.Core.Exceptions;

public class BusinessValidationException : NonRetryableException
{
    public BusinessValidationException(string message) : base(message)
    {
    }
}
