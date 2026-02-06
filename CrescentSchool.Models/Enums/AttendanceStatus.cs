namespace CrescentSchool.Models.Enums
{
    public enum AttendanceStatus
    {
        Attend = 1,
        AbsentStudent = 2,
        AbsentInstructor = 3,
        CancelledByInstructor = 4,
        CancelledByStudent = 5,
        StudentLate5Minutes = 6,
        StudentLate10Minutes = 7,
        InstructorLate5Minutes = 8,
        InstructorLate10Minutes = 9,
    }
}
