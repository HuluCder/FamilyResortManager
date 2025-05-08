using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

/// <summary>
/// Валидатор для обновления клиента
/// </summary>
public class ClientUpdateRequestDtoValidator : AbstractValidator<ClientUpdateRequestDto>
{
    public ClientUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");

        Include(new ClientCreateRequestDtoValidator());
        // Переиспользуем правила из создания для полей Name, Email, Phone, Info
    }
}