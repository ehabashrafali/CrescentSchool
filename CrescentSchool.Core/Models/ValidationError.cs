using CrescentSchool.Core.Enums;
using CrescentSchool.Core.Extensions;
using CrescentSchool.Core.Validation;
using Newtonsoft.Json;

namespace CrescentSchool.Core.Models;

public class ValidationError
{
    public ValidationError(string propertyName, string message, string? errorCode,
        int? index = default)
    {
        Message = message;
        ErrorCode = errorCode.IsNullOrEmpty()
            ? ValidationErrorCode.ModelValidation
                .ToString("G")
            : errorCode!;
        Index = propertyName.GetValidationKeyIndex();
    }

    public ValidationError(string propertyName, string message, ValidationErrorCode errorCode,
        int? index = default)
    {
        Message = message;
        ErrorCode = errorCode.ToString();
        Index = propertyName.GetValidationKeyIndex();
    }

    public string Message { get; set; }
    public string ErrorCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Index { get; set; }
}