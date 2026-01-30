using FluentValidation;

namespace AccommodationBooking.Application.Authentication.Queries.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Haslo jest wymagane.");
        }
    }
}
