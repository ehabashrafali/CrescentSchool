using CrescentSchool.API.ExceptionHandling;

namespace CrescentSchool.API.Extensions;

public class ConflictException : NonRetryableException
{
    public Guid? LatestVersion { get; set; }
    public ConflictException()
        : base("")
    {
    }
    public ConflictException(string message)
        : base(message)
    {
    }
    public ConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    public ConflictException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exists.")
    {
    }
}
