using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models;

public class Session
{
    private Session()
    { }
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Course? Course { get; init; }
    public Instructor? Instructor { get; init; }
    public Student Student { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    private int AllowedDelay => 5;
    public Guid? CourseId { get; set; }
    public Guid? InstructorId { get; set; }

    public Session(DateTime startTime, DateTime? endTime, Guid? courseId, Guid? instructorId, Student student)
    {
        if (endTime.HasValue && endTime <= startTime)
            throw new ArgumentException("End time must be after start time.");

        Student = student ?? throw new ArgumentNullException(nameof(student));
        Id = Guid.NewGuid();
        StartTime = startTime;
        EndTime = endTime;
        CreatedAt = DateTime.UtcNow;
        CourseId = courseId;
        InstructorId = instructorId;
    }
    public void SetStatus(int delayedTime)
    {
        Status = delayedTime > AllowedDelay
                ? AttendanceStatus.AbsentStudent
                : AttendanceStatus.Attend;
    }

}
