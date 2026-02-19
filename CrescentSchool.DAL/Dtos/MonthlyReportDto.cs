using CrescentSchool.DAL.Entities;
using CrescentSchool.Models;
using CrescentSchool.Models.Enums;
using System.Text.Json.Serialization;

namespace CrescentSchool.DAL.Dtos;

public class MonthlyReportDto
{
    [JsonIgnore] public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public QuranSurah? Memorization { get; set; }
    public Grade? MemorizationGrade { get; set; }
    public QuranSurah? Reading { get; set; }
    public Grade? ReadingGrade { get; set; }
    public int NoOfMemorizationAyah { get; set; }
    public int NoOfReadingAyah { get; set; }
    public Grade? BasicQuranRecitationRulesProgress { get; set; }
    public List<Tajweed> TajweedRules { get; set; } = [];
    public Grade? TajweedRulesProgress { get; set; }
    public string? QuranComments { get; set; } = string.Empty;
    public string? IslamicStudiesComments { get; set; } = string.Empty;
    public string? IslamicStudiesTopics { get; set; } = string.Empty;
    public List<IslamicStudiesBook> IslamicStudiesBooks { get; set; } = [];
    public List<BasicQuranRecitationRule> BasicQuranRecitationRules { get; set; } = [];

    public Grade? IslamicStudiesProgress { get; set; }
    public string OthersIslamicStudiesBooks { get; set; } = string.Empty;
}