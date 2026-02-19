using CrescentSchool.Models.Enums;
using System.Text.Json.Serialization;

namespace CrescentSchool.DAL.Entities;

public class IslamicStudiesBook()
{
    public int Id { get; set; }

    public IslamicStudiesBooks Book { get; set; }
    [JsonIgnore]
    public ICollection<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
}