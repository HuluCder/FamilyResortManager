using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators;

public class CleaningTaskCreateRequestDtoValidator : AbstractValidator<CleaningTaskCreateRequestDto>, IValidator<CleaningTaskUpdateRequestDto>
{
    public CleaningTaskCreateRequestDtoValidator()
    {
        RuleFor(x => x.RoomId).GreaterThan(0);
        RuleFor(x => x.StaffId).GreaterThan(0);
        RuleFor(x => x.Status).NotEmpty();
    }

    public ValidationResult Validate(CleaningTaskUpdateRequestDto instance)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(CleaningTaskUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}