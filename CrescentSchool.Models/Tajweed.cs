using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models;

public class Tajweed()
{
    public int Id { get; set; }
    public TajweedRules TajweedRule { get; set; }
    public ICollection<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
}
