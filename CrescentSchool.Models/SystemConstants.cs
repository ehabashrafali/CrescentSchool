namespace CrescentSchool.Models;

public static class SystemConstants
{
    //
    // Summary:
    //     Represents the system default user identifier.
    public static Guid UserId => Guid.Parse("00000000-0000-0000-0000-000000000001");

    //
    // Summary:
    //     Represents the current system time in UTC.
    public static DateTime Now => DateTime.UtcNow;

    //
    // Summary:
    //     Formats a DateTime object into a string representation with the format "MMM dd,
    //     yyyy hh:mm tt".
    //
    // Parameters:
    //   dateTime:
    //     The DateTime object to be formatted.
    //
    // Returns:
    //     A string representation of the DateTime object in the specified format.
    public static string ToStringFormatted(this DateTime dateTime)
    {
        return dateTime.ToString("MMM dd, yyyy hh:mm tt");
    }

    //
    // Summary:
    //     Formats a DateTime object into a string representation with the format "MMM dd,
    //     yyyy hh:mm tt (UTC)".
    //
    // Parameters:
    //   dateTime:
    //     The DateTime object to be formatted.
    //
    // Returns:
    //     A string representation of the DateTime object in the specified format with UTC
    //     appended.
    public static string ToStringFormattedUtc(this DateTime dateTime)
    {
        return dateTime.ToString("MMM dd, yyyy hh:mm tt") + " (UTC)";
    }
}

