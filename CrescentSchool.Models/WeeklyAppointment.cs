namespace CrescentSchool.Models;

public class WeeklyAppointment
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public string Day { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
}
