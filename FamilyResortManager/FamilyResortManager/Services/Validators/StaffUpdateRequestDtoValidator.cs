using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

public class StaffUpdateRequestDtoValidator : AbstractValidator<StaffUpdateRequestDto>
{
    public StaffUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        Include(new StaffCreateRequestDtoValidator());
    }
}