using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

public class ShiftUpdateRequestDtoValidator : AbstractValidator<ShiftUpdateRequestDto>
{
    public ShiftUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        Include(new ShiftCreateRequestDtoValidator());
    }
}