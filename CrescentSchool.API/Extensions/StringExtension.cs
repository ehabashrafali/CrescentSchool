namespace CrescentSchool.API.Extensions;

public static class StringExtension
{
    public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);
    public static bool IsNullOrWhiteSpace(this string? s) => string.IsNullOrWhiteSpace(s);
}
