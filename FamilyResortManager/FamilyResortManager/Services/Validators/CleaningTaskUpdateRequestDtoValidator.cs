using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

public class CleaningTaskUpdateRequestDtoValidator : AbstractValidator<CleaningTaskUpdateRequestDto>
{
    public CleaningTaskUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        Include(new CleaningTaskCreateRequestDtoValidator());
    }
}