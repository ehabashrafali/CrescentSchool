using CrescentSchool.API.ExceptionHandling;
using CrescentSchool.API.Models;

namespace CrescentSchool.API.Exceptions;

public class ValidationException : NonRetryableException
{
    public IReadOnlyDictionary<string, ValidationError[]> ErrorsDictionary { get; private set; } =
        new Dictionary<string, ValidationError[]>();

    public ValidationException(IReadOnlyDictionary<string, ValidationError[]> errorsDictionary) =>
        ErrorsDictionary = errorsDictionary;

    public ValidationException(string errors)
        : base(errors)
    {
    }
}