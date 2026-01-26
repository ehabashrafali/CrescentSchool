using CrescentSchool.BLL.Enums;

namespace CrescentSchool.BLL.DTOs
{
    public class InstructorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public DateOnly DateOfBirth { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public bool IsActive { get; set; }
        public string Role => Roles.Instructor.ToString();
    }
}
