namespace CrescentSchool.Core.Validation;

public static class ValidatorExtensions
{
    public static string ToValidationPropertyKey(this string key)
    {
        if (!key.Contains('[') || !key.Contains(']')) return key;
        var startIndex = key.IndexOf("[", StringComparison.Ordinal);
        var endIndex = key.IndexOf("]", StringComparison.Ordinal);
        var end = key[(endIndex + 1)..];
        return key[..startIndex] + (end.Contains('[') ? end.ToValidationPropertyKey() : end);
    }

    public static int? GetValidationKeyIndex(this string key)
    {
        if (!key.Contains('[') || !key.Contains(']')) return null;
        var index = int.TryParse(key.Split("[")[1].Split("]")[0], out var indexKey) ? indexKey : 0;
        return index;
    }
}
