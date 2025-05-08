using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators
{
    /// <summary>
    /// Валидатор для создания клиента
    /// </summary>
    public class ClientCreateRequestDtoValidator : AbstractValidator<ClientCreateRequestDto>, IValidator<ClientUpdateRequestDto>
    {
        public ClientCreateRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+?\d{7,15}$")
                .WithMessage("Phone must contain only digits and may start with '+'; length between 7 and 15 characters.");

            RuleFor(x => x.Info)
                .MaximumLength(500).WithMessage("Info must not exceed 500 characters.");
        }

        public ValidationResult Validate(ClientUpdateRequestDto instance)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(ClientUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}