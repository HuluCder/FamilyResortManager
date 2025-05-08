using FamilyResortManager.Services.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Validators;

public class BookingCreateRequestDtoValidator : AbstractValidator<BookingCreateRequestDto>, IValidator<BookingUpdateRequestDto>
{
    public BookingCreateRequestDtoValidator()
    {
        RuleFor(x => x.RoomId).GreaterThan(0);
        RuleFor(x => x.ClientId).GreaterThan(0);
        RuleFor(x => x.CheckInDate).LessThan(x => x.CheckOutDate);
        RuleFor(x => x.Status).NotEmpty();
    }

    public ValidationResult Validate(BookingUpdateRequestDto instance)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(BookingUpdateRequestDto instance, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}