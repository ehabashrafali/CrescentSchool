using CrescentSchool.BLL.DTOs;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Repositories;

namespace CrescentSchool.BLL.Services;

public class InstructorService(IInstructorsRepository instructorsRepository) : IInsructorService
{
    public async Task<Guid> CreateInstructorAsync(CreateInstructorDto createInstructorDto, CancellationToken cancellationToken)
        => await instructorsRepository.CreateInstructorAsync(createInstructorDto, cancellationToken);
    public Task DeactivateInstructorAsync(Guid id, CancellationToken cancellationToken)
     => instructorsRepository.DeactivateInstructor(id, cancellationToken);
    public async Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId)
    {
        var instructor = await instructorsRepository.GetByIdAsync(instructorId);

        if (instructor is null)
            return new InstructorDto();

        return new InstructorDto
        {
            Id = instructor.Id,
            FirstName = instructor.User.FirstName,
            LastName = instructor.User.LastName,
            Email = instructor.User.Email,
            PhoneNumber = instructor.User.PhoneNumber,
            IsActive = instructor.User.IsActive,
            Country = instructor.Country,
            Fees = instructor.Fees,
            ZoomMeeting = instructor.ZoomMeeting
        };
    }
    public async Task<List<InstructorDto>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken)
    {
        var instructors = await instructorsRepository.GetInstructorsAsync(instructorIds, cancellationToken);
        return [.. instructors.Select(i => new InstructorDto
        {
            Id = i.Id,
            FirstName = i.User.FirstName,
            LastName = i.User.LastName,
            Email = i.User.Email,
            PhoneNumber = i.User.PhoneNumber,
            IsActive = i.User.IsActive,
            Country = i.Country,
            Fees = i.Fees,
            ZoomMeeting = i.ZoomMeeting
        })];
    }
    public async Task<List<StudentDto>> GetInstructorStudents(Guid id)
    {
        var students = await instructorsRepository.GetInstuctorStudents(id);
        var studentDto = students.Select(s => new StudentDto
        {
            Id = s.Id,
            FirstName = s.User.FirstName,
            LastName = s.User.LastName,
            Email = s.User.Email,
            Country = s.User.Country,
            PhoneNumber = s.User.PhoneNumber,
            DateOfBirth = s.User.DateOfBirth,
            ZoomMeeting = s.ZoomMeeting,
            MonthlyReportDtos = [.. s.StudentMonthlyReports.Select(r => new MonthlyReportDto
            {
                Id = r.Id,
                Date = r.Date,
                Memorization = r.Memorization,
                Reading = r.Reading,
                NoOfMemorizationAyah = r.NoOfMemorizationAyah,
                MemorizationGrade = r.MemorizationGrade,
                NoOfReadingAyah = r.NoOfReadingAyah,
                ReadingGrade = r.ReadingGrade,
                BasicQuranRecitationRulesProgress = r.BasicQuranRecitationRulesProgress,
                TajweedRules = r.TajweedRules,
                TajweedRulesProgress = r.TajweedRulesProgress,
                QuranComments = r.QuranComments,
                IslamicStudiesComments = r.IslamicStudiesComments,
                IslamicStudiesProgress = r.IslamicStudiesProgress,
                IslamicStudiesTopics = r.IslamicStudiesTopics,
                IslamicStudiesBooks = r.IslamicStudiesBooks,
                BasicQuranRecitationRules = r.BasicQuranRecitationRules,
                OthersIslamicStudiesBooks = r.OthersIslamicStudiesBooks
            })]
        }).ToList();
        return studentDto;
    }
}
