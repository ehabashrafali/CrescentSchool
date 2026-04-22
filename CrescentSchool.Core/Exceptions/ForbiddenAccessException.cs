using CrescentSchool.Core.ExceptionHandling;
using System.Runtime.Serialization;

namespace CrescentSchool.Core.Exceptions;

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

    protected ForbiddenAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}