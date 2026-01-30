using FluentValidation;

namespace AccommodationBooking.Application.Authentication.Commands.UpdateEmail
{
    public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
    {
        public UpdateEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
                .EmailAddress().WithMessage("Adres e-mail jest nieprawidlowy.");
        }
    }
}
