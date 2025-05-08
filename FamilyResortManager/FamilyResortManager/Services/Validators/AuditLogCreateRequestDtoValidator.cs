using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

public class AuditLogCreateRequestDtoValidator : AbstractValidator<AuditLogCreateRequestDto>
{
    public AuditLogCreateRequestDtoValidator()
    {
        RuleFor(x => x.Action).NotEmpty();
        RuleFor(x => x.Details).NotEmpty();
    }
}