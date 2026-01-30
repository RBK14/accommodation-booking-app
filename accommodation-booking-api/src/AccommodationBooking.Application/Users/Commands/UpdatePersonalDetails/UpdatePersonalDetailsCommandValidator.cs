using FluentValidation;

namespace AccommodationBooking.Application.Users.Commands.UpdatePersonalDetails
{
    /// <summary>
    /// Validator for UpdatePersonalDetailsCommand.
    /// </summary>
    public class UpdatePersonalDetailsCommandValidator : AbstractValidator<UpdatePersonalDetailsCommand>
    {
        public UpdatePersonalDetailsCommandValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imie jest wymagane.")
                .MaximumLength(100).WithMessage("Imie nie moze byc dluzsze niz 100 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(100).WithMessage("Nazwisko nie moze byc dluzsze niz 100 znaków.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Numer telefonu jest nieprawidlowy.");
        }
    }
}
