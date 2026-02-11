using FluentValidation;
using RestaurantReservation.Host.Dtos;

namespace RestaurantReservation.Host.Validation;

public class ReservationRequestValidator : AbstractValidator<ReservationRequestDto>
{
    public ReservationRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.SlotId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
