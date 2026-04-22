using Microsoft.Extensions.Logging;

namespace CrescentSchool.API.Extensions;

public static class LoggingExtensions
{
    public static void LogException<T, TException>(this ILogger<T> logger, TException exception,
        LogLevel logLevel = LogLevel.Error)
        where TException : Exception
    {
        logger.Log(logLevel, "Exception => {@Exception}.", exception);
    }

    public static void LogException<T, TException>(this ILogger<T> logger,
        string message,
        TException exception,
        LogLevel logLevel = LogLevel.Error)
        where TException : Exception
    {
        logger.Log(logLevel, "{Message} \n Exception => {@Exception}.",
            message,
            exception);
    }
}
