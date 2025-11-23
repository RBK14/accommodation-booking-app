using FluentValidation;

namespace AccommodationBooking.Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.CheckIn)
                .NotEmpty().WithMessage("Data zameldowania jest wymagana.")
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Data zameldowania nie może być datą z przeszłości.");

            RuleFor(x => x.CheckOut)
                .NotEmpty().WithMessage("Data wymeldowania jest wymagana.")
                .GreaterThan(x => x.CheckIn)
                .WithMessage("Data wymeldowania musi być późniejsza niż data zameldowania.");
        }
    }
}