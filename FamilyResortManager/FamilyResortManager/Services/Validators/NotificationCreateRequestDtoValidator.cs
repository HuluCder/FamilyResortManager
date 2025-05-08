using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators;

public class NotificationCreateRequestDtoValidator : AbstractValidator<NotificationCreateRequestDto>, IValidator<NotificationUpdateRequestDto>
{
    public NotificationCreateRequestDtoValidator()
    {
        RuleFor(x => x.Message).NotEmpty();
    }

    public ValidationResult Validate(NotificationUpdateRequestDto instance)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(NotificationUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}