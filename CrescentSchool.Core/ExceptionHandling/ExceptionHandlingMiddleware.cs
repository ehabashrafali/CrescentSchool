using CrescentSchool.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CrescentSchool.Core.ExceptionHandling;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NonRetryableException e)
        {
            _logger.LogException(e, LogLevel.Warning);
            await HandleExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            _logger.LogException(e);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var response = exception.ToErrorResponse();
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = response.ErrorCode.ToStatusCode();
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response,
            new Newtonsoft.Json.Converters.StringEnumConverter()));
    }
}