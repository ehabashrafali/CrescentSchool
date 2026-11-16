using CrescentSchool.BLL.DTOs;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.Models.Enums;

namespace CrescentSchool.BLL.Mappings;

internal static class DtoMappings
{
    public static MonthlyReportViewDto ToMonthlyReportViewDto(this StudentMonthlyReport r, Guid studentId, bool referenceValuesOnly = false) => new()
    {
        Id = r.Id,
        StudentId = studentId,
        Date = r.Date,
        Memorization = r.Memorization,
        Reading = r.Reading,
        NoOfMemorizationAyah = r.NoOfMemorizationAyah,
        MemorizationGrade = r.MemorizationGrade,
        NoOfReadingAyah = r.NoOfReadingAyah,
        ReadingGrade = r.ReadingGrade,
        BasicQuranRecitationRulesProgress = r.BasicQuranRecitationRulesProgress,
        BasicQuranRecitationRules = r.BasicQuranRecitationRules,
        TajweedRules = r.TajweedRules,
        TajweedRulesProgress = r.TajweedRulesProgress,
        QuranComments = r.QuranComments,
        IslamicStudiesComments = r.IslamicStudiesComments,
        IslamicStudiesTopics = r.IslamicStudiesTopics,
        IslamicStudiesBooks = r.IslamicStudiesBooks,
        IslamicStudiesProgress = r.IslamicStudiesProgress,
        OthersIslamicStudiesBooks = r.OthersIslamicStudiesBooks
    };

    public static StudentDto ToStudentDto(this Student student, bool includeReports) => new()
    {
        Id = student.Id,
        FirstName = student.User.FirstName,
        LastName = student.User.LastName,
        Email = student.User.Email,
        Country = student.User.Country,
        PhoneNumber = student.User.PhoneNumber,
        DateOfBirth = student.User.DateOfBirth,
        IsActive = student.Status == StudentStatus.Active,
        InstructorId = student.InstructorId,
        Fees = student.Fees,
        MonthlyReportDtos = includeReports
            ? [.. student.StudentMonthlyReports.Select(r => r.ToMonthlyReportViewDto(student.Id))]
            : [],
        WeeklyAppointments = [.. (student.WeeklyAppointments ?? []).Select(wa => new WeeklyAppointmentDto
        {
            Day = wa.Day,
            Time = wa.Time,
            TimeZone = wa.TimeZone
        })]
    };

    public static InstructorDto ToInstructorDto(this Instructor instructor) => new()
    {
        Id = instructor.Id,
        FirstName = instructor.User.FirstName,
        LastName = instructor.User.LastName,
        Email = instructor.User.Email,
        PhoneNumber = instructor.User.PhoneNumber,
        IsActive = instructor.User.IsActive,
        Country = instructor.User.Country,
        Fees = instructor.Fees
    };
}
