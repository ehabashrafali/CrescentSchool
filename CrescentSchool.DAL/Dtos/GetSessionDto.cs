namespace CrescentSchool.DAL.Dtos;

public class GetSessionDto : SessionDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
}
