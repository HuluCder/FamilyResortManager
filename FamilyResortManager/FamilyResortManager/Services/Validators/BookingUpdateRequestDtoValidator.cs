using FamilyResortManager.Services.DTOs;
using FluentValidation;

namespace FamilyResortManager.Services.Validators;

public class BookingUpdateRequestDtoValidator 
    : AbstractValidator<BookingUpdateRequestDto>
{
    public BookingUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id бронирования обязательно.");

        RuleFor(x => x.RoomId)
            .GreaterThan(0).WithMessage("Нужно выбрать номер.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Нужно выбрать клиента.");

        RuleFor(x => x.CheckInDate)
            .NotEmpty().WithMessage("Дата заезда обязательна.");

        RuleFor(x => x.CheckOutDate)
            .NotEmpty().WithMessage("Дата выезда обязательна.")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Дата выезда должна быть позже даты заезда.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Статус обязателен.");
    }
}
