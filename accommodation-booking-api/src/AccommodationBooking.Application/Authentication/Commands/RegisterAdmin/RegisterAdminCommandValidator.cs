using FluentValidation;

namespace AccommodationBooking.Application.Authentication.Commands.RegisterAdmin
{
    public class RegisterAdminCommandValidator : AbstractValidator<RegisterAdminCommand>
    {
        public RegisterAdminCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
                .EmailAddress().WithMessage("Adres e-mail jest nieprawidlowy.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Haslo jest wymagane.")
                .MinimumLength(8).WithMessage("Haslo musi miec co najmniej 8 znaków.")
                .Matches(@"[A-Z]").WithMessage("Haslo musi zawierac co najmniej jedna wielka litere.")
                .Matches(@"[a-z]").WithMessage("Haslo musi zawierac co najmniej jedna mala litere.")
                .Matches(@"\d").WithMessage("Haslo musi zawierac co najmniej jedna cyfre.")
                .Matches(@"[\W_]").WithMessage("Haslo musi zawierac co najmniej jeden znak specjalny.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imie jest wymagane.")
                .MaximumLength(50).WithMessage("Imie nie moze byc dluzsze niz 50 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(50).WithMessage("Nzwisko nie moze byc dluzsze niz 50 znaków.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Numer telefonu jest nieprawidlowy.");
        }
    }
}
