using FluentValidation;

namespace AccommodationBooking.Application.Users.Commands.UpdatePesonalDetails
{
    public class UpdatePersonalDetailsCommandValidator : AbstractValidator<UpdatePersonalDetailsCommand>
    {
        public UpdatePersonalDetailsCommandValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(100).WithMessage("Imię nie może być dłuższe niż 100 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(100).WithMessage("Nzwisko nie może być dłuższe niż 100 znaków.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Numer telefonu jest nieprawidłowy.");
        }
    }
}
