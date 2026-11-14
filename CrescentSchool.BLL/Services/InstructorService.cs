using CrescentSchool.BLL.DTOs;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.BLL.Mappings;
using CrescentSchool.Core.Extensions;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Enums;
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
        var user = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new NotFoundException("Instructor not found");

        user.IsDeleted = true;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception("Failed to delete user");
    }

    public async Task<InstructorDto> GetInstructorByIdAsync(Guid instructorId, CancellationToken cancellationToken)
    {
        var instructor = await instructorsRepository.GetByIdAsync(instructorId, cancellationToken);

        return instructor is null ? new InstructorDto() : instructor.ToInstructorDto();
    }

    public async Task<List<InstructorDto>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken)
    {
        var instructors = await instructorsRepository.GetInstructorsAsync(instructorIds, cancellationToken);
        return [.. instructors.Select(i => i.ToInstructorDto())];
    }
    public async Task<List<StudentDto>> GetInstructorStudents(Guid id, CancellationToken cancellationToken)
    {
        var students = await instructorsRepository.GetInstructorStudents(id, cancellationToken);
        return [.. students.Select(s => s.ToStudentDto(includeReports: true))];
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

        var instructor = await instructorsRepository.GetByIdAsync(id, cancellationToken);
        if (instructor is null)
            return Guid.Empty;

        instructor.Fees = updateInstructorDto.Fees;

        await instructorsRepository.UpdateInstructor(instructor, cancellationToken);

        return instructor.Id;
    }
    private async Task ChangePasswordAsync(ApplicationUser user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
