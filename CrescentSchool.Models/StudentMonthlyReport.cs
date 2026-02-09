using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models;

public class StudentMonthlyReport
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public QuranSurah? Memorization { get; set; }
    public Grade? MemorizationGrade { get; set; }
    public QuranSurah? Reading { get; set; }
    public Grade ReadingGrade { get; set; }
    public int NoOfMemorizationAyah { get; set; }
    public int NoOfReadingAyah { get; set; }
    public Grade? BasicQuranRecitationRulesProgress { get; set; }
    public Grade? TajweedRulesProgress { get; set; }
    public string? QuranComments { get; set; } = string.Empty;
    public string? IslamicStudiesComments { get; set; } = string.Empty;
    public string? IslamicStudiesTopics { get; set; } = string.Empty;
    public Grade? IslamicStudiesProgress { get; set; }
    public Student Student { get; set; } = null!;
    public List<Tajweed> TajweedRules { get; set; } = [];
    public List<BasicQuranRecitationRule> BasicQuranRecitationRules { get; set; } = [];
    public List<IslamicStudiesBook> IslamicStudiesBooks { get; set; } = [];
}
