using FluentValidation;

namespace CrescentSchool.BLL.Validation;

public record RescheduleSessionValidation(DateTime CurrentSessionDateTime, DateTime NewSessionDateTime);

public class RescheduleSessionValidator : AbstractValidator<RescheduleSessionValidation>
{
    public RescheduleSessionValidator(TimeProvider timeProvider)
    {
        // Persisted session timestamps are UTC, including when EF returns an unspecified Kind.
        RuleFor(x => x.CurrentSessionDateTime)
            .Must(current =>
                DateTime.SpecifyKind(current, DateTimeKind.Utc)
                    - timeProvider.GetUtcNow().UtcDateTime >= TimeSpan.FromHours(2))
            .WithMessage("Sessions can only be rescheduled at least two hours before their current start time.")
            .WithErrorCode("RescheduleWindowClosed");

        RuleFor(x => x)
            .Must(x =>
                DateTime.SpecifyKind(x.NewSessionDateTime, DateTimeKind.Utc)
                    > DateTime.SpecifyKind(x.CurrentSessionDateTime, DateTimeKind.Utc))
            .WithMessage("Sessions cannot be rescheduled to a time before the current session time.")
            .WithErrorCode("RescheduleBeforeCurrentSession");
    }
}
