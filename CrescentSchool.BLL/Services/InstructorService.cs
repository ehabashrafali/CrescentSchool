using CrescentSchool.BLL.DTOs;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Repositories;

namespace CrescentSchool.BLL.Services;

public class InstructorService(IInstructorsRepository instructorsRepository) : IInsructorService
{
    public async Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId)
    {
        var instructor = await instructorsRepository.GetByIdAsync(instructorId);

        if (instructor is null)
            return new InstructorDto();

        return new InstructorDto
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Email = instructor.Email,
            PhoneNumber = instructor.PhoneNumber,
            IsActive = instructor.IsActive,
            Country = instructor.Country,

        };
    }
    public async Task<List<StudentDto>> GetInstructorStudents(Guid id)
    {
        var students = await instructorsRepository.GetInstuctorStudents(id);
        var studentDto = students.Select(s => new StudentDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            Country = s.Country,
            PhoneNumber = s.PhoneNumber,
            DateOfBirth = s.DateOfBirth,
            ZoomMeeting = s.ZoomMeeting,
            MonthlyReportDtos = [.. s.StudentMonthlyReports.Select(r => new Models.Dtos.MonthlyReportDto
            {
                Id = r.Id,
                Date = r.Date,
                Memorization = r.Memorization,
                Reading = r.Reading,
                NoOfMemorizationAyah = r.NoOfMemorizationAyah,
                NoOfReadingAyah = r.NoOfReadingAyah,
                Grade = r.Grade,
                BasicQuranRecitationRules = r.BasicQuranRecitationRules,
                TajweedRules = r.TajweedRules,
                Progress = r.Progress,
                QuranComments = r.QuranComments,
                IslamicStudiesComments = r.IslamicStudiesComments,
                IslamicStudiesTopics = r.IslamicStudiesTopics,
                IslamicStudiesBooks = r.IslamicStudiesBooks,
                IslamicStudiesProgress = r.IslamicStudiesProgress
            })]


        }).ToList();
        return studentDto;
    }
}
