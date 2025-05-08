using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators;

public class StaffCreateRequestDtoValidator : AbstractValidator<StaffCreateRequestDto>, IValidator<StaffUpdateRequestDto>
{
    public StaffCreateRequestDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.HourlyRate).GreaterThan(0);
    }

    public ValidationResult Validate(StaffUpdateRequestDto instance)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(StaffUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}