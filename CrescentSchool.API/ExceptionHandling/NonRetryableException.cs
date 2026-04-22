namespace CrescentSchool.API.ExceptionHandling;

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
}
