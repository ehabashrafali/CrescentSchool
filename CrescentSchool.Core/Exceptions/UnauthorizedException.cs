
using CrescentSchool.Core.ExceptionHandling;
using System.Runtime.Serialization;

public class UnauthorizedException : NonRetryableException
{
    public UnauthorizedException()
        : base()
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public UnauthorizedException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }

    protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
