namespace CrescentSchool.DAL.Dtos;

public class GetSessionsResult
{
    public List<GetSessionDto> Sessions { get; set; }
    public int TotalCount { get; set; }
}
