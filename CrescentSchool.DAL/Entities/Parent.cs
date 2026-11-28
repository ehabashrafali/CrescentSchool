namespace CrescentSchool.DAL.Entities;

public class Parent
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public List<Student> Students { get; set; } = [];
}
