using System.Runtime.Serialization;

namespace CrescentSchool.Core.ExceptionHandling;

public class NonRetryableException : Exception
{
    protected NonRetryableException()
    {
    }

    protected NonRetryableException(string message) : base(message)
    {
    }

    protected NonRetryableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected NonRetryableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}