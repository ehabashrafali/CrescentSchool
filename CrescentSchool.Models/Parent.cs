namespace CrescentSchool.Models;

public class Parent : AuditableEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = [];
    public bool IsActive { get; set; }
}
