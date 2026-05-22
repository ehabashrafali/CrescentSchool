namespace CrescentSchool.DAL.Dtos;

/// <summary>
/// DTO used when creating a new monthly report.
/// Inherits all report fields from <see cref="MonthlyReportDto"/>.
/// The Id is intentionally not exposed because it is generated server-side.
/// </summary>
public class CreateMonthlyReportDto : MonthlyReportDto
{
}
