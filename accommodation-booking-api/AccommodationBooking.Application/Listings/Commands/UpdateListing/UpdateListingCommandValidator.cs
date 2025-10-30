using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using FluentValidation;

namespace AccommodationBooking.Application.Listings.Commands.UpdateListing
{
    public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
    {
        public UpdateListingCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tytuł jest wymagany.")
                .MaximumLength(200).WithMessage("Nazwa nie może być dłuższa niż 200 znaków.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Opis jest wymagany.")
                .MaximumLength(2000).WithMessage("Opis nie może być dłuższy niż 2000 znaków.");

            RuleFor(x => x.AccommodationType)
                .NotEmpty().WithMessage("Typ zakwaterowania jest wymagany.")
                .Must(AccommodationTypeExtensions.IsValidAccommodationType)
                .WithMessage("Nieprawidłowy typ zakwaterowania.");

            RuleFor(x => x.Beds)
                .GreaterThan(0).WithMessage("Liczba łóżek musi być większa od zera.");

            RuleFor(x => x.MaxGuests)
                .GreaterThan(0).WithMessage("Liczba gości musi być większa od zera.")
                .GreaterThanOrEqualTo(x => x.Beds)
                .WithMessage("Liczba gości nie może być mniejsza niż liczba łóżek.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Kraj jest wymagany.")
                .MaximumLength(100).WithMessage("Nazwa kraju nie może być dłuższa niż 100 znaków.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Miasto jest wymagane.")
                .MaximumLength(100).WithMessage("Nazwa miasta nie może być dłuższa niż 100 znaków.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Kod pocztowy jest wymagany.")
                .Matches(@"^[0-9A-Za-z\- ]{3,10}$").WithMessage("Kod pocztowy ma nieprawidłowy format.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Ulica jest wymagana.")
                .MaximumLength(150).WithMessage("Nazwa ulicy nie może być dłuższa niż 150 znaków.");

            RuleFor(x => x.BuildingNumber)
                .NotEmpty().WithMessage("Numer budynku jest wymagany.")
                .MaximumLength(20).WithMessage("Numer budynku nie może być dłuższy niż 20 znaków.");

            RuleFor(x => x.AmountPerDay)
                .GreaterThan(0).WithMessage("Kwota za dzień musi być większa od zera.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Waluta jest wymagana.")
                .Must(CurrencyExtensions.IsValidCurrency)
                .WithMessage("Nieprawidłowy kod waluty.");
        }
    }
}
