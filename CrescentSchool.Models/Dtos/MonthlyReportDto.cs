using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models.Dtos;

public class MonthlyReportDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public QuranSurah Memorization { get; set; }
    public QuranSurah Reading { get; set; }
    public int NoOfMemorizationAyah { get; set; }
    public int NoOfReadingAyah { get; set; }
    public Grade Grade { get; set; }
    public QuranRecitationTopic BasicQuranRecitationRules { get; set; }
    public TajweedRules TajweedRules { get; set; }
    public Grade Progress { get; set; }
    public string QuranComments { get; set; } = string.Empty;
    public string IslamicStudiesComments { get; set; } = string.Empty;
    public string IslamicStudiesTopics { get; set; } = string.Empty;
    public IslamicStudiesBooks IslamicStudiesBooks { get; set; }
    public Grade IslamicStudiesProgress { get; set; }
}