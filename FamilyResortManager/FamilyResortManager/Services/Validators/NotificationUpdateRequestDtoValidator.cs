using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

public class NotificationUpdateRequestDtoValidator : AbstractValidator<NotificationUpdateRequestDto>
{
    public NotificationUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        Include(new NotificationCreateRequestDtoValidator());
    }
}