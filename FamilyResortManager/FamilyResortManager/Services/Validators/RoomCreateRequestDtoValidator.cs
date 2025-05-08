using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators
{
    /// <summary>
    /// Валидатор для создания номера
    /// </summary>
    public class RoomCreateRequestDtoValidator : AbstractValidator<RoomCreateRequestDto>, IValidator<RoomUpdateRequestDto>
    {
        public RoomCreateRequestDtoValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Number is required.")
                .MaximumLength(20).WithMessage("Number must not exceed 20 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .MaximumLength(50).WithMessage("Type must not exceed 50 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(20).WithMessage("Status must not exceed 20 characters.");
        }

        public ValidationResult Validate(RoomUpdateRequestDto instance)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(RoomUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}