using CrescentSchool.Models.Enums;
using FluentValidation;

namespace CrescentSchool.BLL.Validation;

public record CancelSessionValidation(DateTime SessionDateTime, Role Role);

public class CancelSessionValidator : AbstractValidator<CancelSessionValidation>
{
    public CancelSessionValidator(TimeProvider timeProvider)
    {
        RuleFor(request => request.SessionDateTime)
            .Must((request, date) =>
                DateTime.SpecifyKind(date, DateTimeKind.Utc) - timeProvider.GetUtcNow().UtcDateTime
                    >= TimeSpan.FromHours(request.Role == Role.Student ? 2 : 6))
            .When(request => request.Role is Role.Student or Role.Instructor)
            .WithMessage(request => request.Role == Role.Student
                ? "Students can only cancel sessions at least two hours before their start time."
                : "Instructors can only cancel sessions at least six hours before their start time.")
            .WithErrorCode("CancellationWindowClosed");
    }
}
