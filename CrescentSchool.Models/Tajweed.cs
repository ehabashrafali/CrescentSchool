using CrescentSchool.Models.Enums;
using System.Text.Json.Serialization;

namespace CrescentSchool.Models;

public class Tajweed()
{
    public int Id { get; set; }
    public TajweedRules TajweedRule { get; set; }
    [JsonIgnore]
    public ICollection<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
}
