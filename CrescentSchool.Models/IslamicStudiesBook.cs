using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models;

public class IslamicStudiesBook()
{
    public int Id { get; set; }

    public IslamicStudiesBooks Book { get; set; }
    public ICollection<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
}