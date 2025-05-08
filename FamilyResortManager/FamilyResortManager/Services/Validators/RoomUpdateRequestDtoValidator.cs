using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

/// <summary>
/// Валидатор для обновления номера
/// </summary>
public class RoomUpdateRequestDtoValidator : AbstractValidator<RoomUpdateRequestDto>
{
    public RoomUpdateRequestDtoValidator()
    {
       
    }
}