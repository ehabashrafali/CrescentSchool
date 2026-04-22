using CrescentSchool.Core.Enums;

namespace CrescentSchool.Core.Models;

public class ErrorResponse
{
    private ErrorResponse()
    {
    }

    public ErrorResponse(string title, ErrorCode errorCode, string message,
        IReadOnlyDictionary<string, ValidationError[]>? details = null,
        Guid? latestVersion = default)
    {
        Title = title;
        ErrorCode = errorCode;
        Message = message;
        Details = details;
        LatestVersion = latestVersion;
    }

    public string Title { get; set; }

    public ErrorCode ErrorCode { get; set; }

    public string Message { get; set; }
    public Guid? LatestVersion { get; set; }
    public IReadOnlyDictionary<string, ValidationError[]>? Details { get; set; }

    public override string ToString() =>
        $"Title: {Title}, ErrorCode: {ErrorCode}, Message: {Message}";
}