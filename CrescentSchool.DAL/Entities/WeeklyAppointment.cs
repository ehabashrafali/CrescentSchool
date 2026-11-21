using CrescentSchool.Models.Enums;

namespace CrescentSchool.DAL.Entities;

public class WeeklyAppointment
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public SessionDuration Duration { get; set; } = SessionDuration.ThirtyMinutes;
    public string Day { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
}