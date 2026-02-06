using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models;

public class BasicQuranRecitationRule()
{
    public int Id { get; set; }
    public QuranRecitationTopic QuranRecitationTopic { get; set; }
    public ICollection<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
}
