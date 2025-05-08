using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators;

public class ShiftCreateRequestDtoValidator : AbstractValidator<ShiftCreateRequestDto>, IValidator<ShiftUpdateRequestDto>
{
    public ShiftCreateRequestDtoValidator()
    {
        RuleFor(x => x.StaffId).GreaterThan(0);
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
    }

    public ValidationResult Validate(ShiftUpdateRequestDto instance)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(ShiftUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}