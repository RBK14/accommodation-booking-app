using FluentValidation;

namespace AccommodationBooking.Application.Authentication.Commands.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Hasło jest wymagane.");

            RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Nowe hasło jest wymagane.")
                   .MinimumLength(8).WithMessage("Nowe hasło musi mieć co najmniej 8 znaków.")
                   .Matches(@"[A-Z]").WithMessage("Nowe hasło musi zawierać co najmniej jedną wielką literę.")
                   .Matches(@"[a-z]").WithMessage("Nowe hasło musi zawierać co najmniej jedną małą literę.")
                   .Matches(@"\d").WithMessage("Nowe hasło musi zawierać co najmniej jedną cyfrę.")
                   .Matches(@"[\W_]").WithMessage("Nowe hasło musi zawierać co najmniej jeden znak specjalny.");
        }
    }
}
