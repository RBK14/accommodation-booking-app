using FluentValidation;

namespace AccommodationBooking.Application.Authentication.Commands.Common
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
                .EmailAddress().WithMessage("Adres e-mail jest nieprawidłowy.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(8).WithMessage("Hasło musi mieć co najmniej 8 znaków.")
                .Matches(@"[A-Z]").WithMessage("Hasło musi zawierać co najmniej jedną wielką literę.")
                .Matches(@"[a-z]").WithMessage("Hasło musi zawierać co najmniej jedną małą literę.")
                .Matches(@"\d").WithMessage("Hasło musi zawierać co najmniej jedną cyfrę.")
                .Matches(@"[\W_]").WithMessage("Hasło musi zawierać co najmniej jeden znak specjalny.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(50).WithMessage("Imię nie może być dłuższe niż 50 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(50).WithMessage("Nzwisko nie może być dłuższe niż 50 znaków.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Numer telefonu jest nieprawidłowy.");
        }
    }
}
