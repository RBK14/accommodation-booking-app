using FluentValidation;

namespace AccommodationBooking.Application.Authentication.Commands.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Haslo jest wymagane.");

            RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Nowe haslo jest wymagane.")
                   .MinimumLength(8).WithMessage("Nowe haslo musi miec co najmniej 8 znaków.")
                   .Matches(@"[A-Z]").WithMessage("Nowe haslo musi zawierac co najmniej jedna wielka litere.")
                   .Matches(@"[a-z]").WithMessage("Nowe haslo musi zawierac co najmniej jedna mala litere.")
                   .Matches(@"\d").WithMessage("Nowe haslo musi zawierac co najmniej jedna cyfre.")
                   .Matches(@"[\W_]").WithMessage("Nowe haslo musi zawierac co najmniej jeden znak specjalny.");
        }
    }
}
