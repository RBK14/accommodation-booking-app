using AccommodationBooking.Domain.ListingAggregate.Enums;
using AccommodationBooking.Domain.ReservationAggregate.Enums;
using FluentValidation;

namespace AccommodationBooking.Application.Reservations.Commands.UpdateStatus
{
    public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
    {
        public UpdateStatusCommandValidator() 
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status jest wymagany.")
                .Must(ReservationStatusExtensions.IsValidReservationStatus)
                .WithMessage("Nieprawidłowy status");
        }
    }
}
