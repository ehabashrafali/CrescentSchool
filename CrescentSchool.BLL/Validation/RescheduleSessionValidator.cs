using CrescentSchool.DAL.Entities;
using FluentValidation;

namespace CrescentSchool.BLL.Validation;

public class RescheduleSessionValidator : AbstractValidator<UpComingSessions>
{
    public RescheduleSessionValidator(TimeProvider timeProvider)
    {
        // Persisted session timestamps are UTC, including when EF returns an unspecified Kind.
        RuleFor(session => session.SessionDateTime)
            .Must(sessionDateTime =>
                DateTime.SpecifyKind(sessionDateTime, DateTimeKind.Utc)
                    - timeProvider.GetUtcNow().UtcDateTime >= TimeSpan.FromHours(2))
            .WithMessage("Sessions can only be rescheduled at least two hours before their current start time.")
            .WithErrorCode("RescheduleWindowClosed");
    }
}
