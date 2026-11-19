using System.ComponentModel.DataAnnotations;

namespace CrescentSchool.DAL.Dtos;

public class WeeklyAppointmentDto
{
    public string Day { get; set; } = string.Empty;
    [Required]
    [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d(:[0-5]\d)?$",
        ErrorMessage = "Time must be a 24-hour clock time in HH:mm or HH:mm:ss format.")]
    public string Time { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
}
