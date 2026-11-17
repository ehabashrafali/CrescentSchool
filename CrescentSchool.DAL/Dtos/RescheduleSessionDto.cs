using System.ComponentModel.DataAnnotations;

namespace CrescentSchool.DAL.Dtos;

public class RescheduleSessionDto
{
    [Required]
    public long MeetingId { get; set; } 

    [Required]
    public DateTimeOffset? NewDateTime { get; set; }
}
