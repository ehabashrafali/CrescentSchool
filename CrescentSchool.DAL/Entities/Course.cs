using CrescentSchool.Models;

namespace CrescentSchool.DAL.Entities;

public class Course : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DetailedDescription { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public List<Student> Students { get; set; } = [];
    public List<Instructor> Instructors { get; set; } = [];
    public List<Session> Sessions { get; set; } = [];
}