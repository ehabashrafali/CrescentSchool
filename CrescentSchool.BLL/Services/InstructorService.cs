using CrescentSchool.BLL.DTOs;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.Core.Exceptions;
using CrescentSchool.Core.Extensions;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CrescentSchool.BLL.Services;

public class InstructorService(IInstructorsRepository instructorsRepository, UserManager<ApplicationUser> _userManager) : IInstructorService
{
    public async Task<Guid> CreateInstructorAsync(CreateInstructorDto createInstructorDto, CancellationToken cancellationToken)
        => await instructorsRepository.CreateInstructorAsync(createInstructorDto, cancellationToken);
    public Task DeactivateInstructorAsync(Guid id, CancellationToken cancellationToken)
     => instructorsRepository.DeactivateInstructor(id, cancellationToken);

    public async Task DeleteInstructorAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is not null)
            user.IsDeleted = true;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception("Failed to delete user");
    }

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
            Country = instructor.User.Country,
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
            Country = i.User.Country,
            Fees = i.Fees,
            ZoomMeeting = i.ZoomMeeting
        })];
    }
    public async Task<List<StudentDto>> GetInstructorStudents(Guid id)
    {
        var students = await instructorsRepository.GetInstructorStudents(id);
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

    public async Task<Guid> UpdateInstructorAsync(Guid id, UpdateInstructorDto updateInstructorDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            return Guid.Empty;

        user.FirstName = updateInstructorDto.FirstName;
        user.LastName = updateInstructorDto.LastName;
        user.UserName = updateInstructorDto.Email;
        user.NormalizedUserName = updateInstructorDto.Email.ToUpper();
        user.Email = updateInstructorDto.Email;
        user.NormalizedEmail = updateInstructorDto.Email.ToUpper();
        user.PhoneNumber = updateInstructorDto.PhoneNumber;

        if (updateInstructorDto.Password != string.Empty)
            await ChangePasswordAsync(user, updateInstructorDto.Password);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new Exception("Failed to update identity user");

        var instructor = await instructorsRepository.GetByIdAsync(id);
        if (instructor is null)
            return Guid.Empty;

        instructor.ZoomMeeting = updateInstructorDto.ZoomLink;
        instructor.Fees = updateInstructorDto.Fees;

        await instructorsRepository.UpdateInstructor(instructor, cancellationToken);

        return instructor.Id;
    }
    private async Task ChangePasswordAsync(ApplicationUser user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
    }
}
