using CrescentSchool.DAL.Entities;
using CrescentSchool.Models.Enums;
using System.Text.Json.Serialization;

namespace CrescentSchool.Models;

public class BasicQuranRecitationRule()
{
    public int Id { get; set; }
    public QuranRecitationTopic QuranRecitationTopic { get; set; }
    [JsonIgnore]
    public ICollection<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
}
