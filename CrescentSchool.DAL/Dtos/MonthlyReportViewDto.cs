namespace CrescentSchool.DAL.Dtos;

/// <summary>
/// DTO used when returning monthly reports to clients.
/// Inherits all report fields from <see cref="MonthlyReportDto"/> and adds the Id.
/// </summary>
public class MonthlyReportViewDto : MonthlyReportDto
{
    public Guid Id { get; set; }
}
