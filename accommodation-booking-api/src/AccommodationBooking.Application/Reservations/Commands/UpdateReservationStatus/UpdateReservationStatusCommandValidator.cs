using AccommodationBooking.Domain.ReservationAggregate.Enums;
using FluentValidation;

namespace AccommodationBooking.Application.Reservations.Commands.UpdateReservationStatus
{
    public class UpdateReservationStatusCommandValidator : AbstractValidator<UpdateReservationStatusCommand>
    {
        public UpdateReservationStatusCommandValidator() 
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status jest wymagany.")
                .Must(ReservationStatusExtensions.IsValidReservationStatus)
                .WithMessage("Nieprawidlowy status");
        }
    }
}
