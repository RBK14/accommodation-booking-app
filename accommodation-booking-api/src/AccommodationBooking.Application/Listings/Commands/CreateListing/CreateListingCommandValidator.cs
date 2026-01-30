using AccommodationBooking.Domain.Common.Enums;
using AccommodationBooking.Domain.ListingAggregate.Enums;
using FluentValidation;

namespace AccommodationBooking.Application.Listings.Commands.CreateListing
{
    public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
    {
        public CreateListingCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tytul jest wymagany.")
                .MaximumLength(200).WithMessage("Nazwa nie moze byc dluzsza niz 200 znaków.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Opis jest wymagany.")
                .MaximumLength(2000).WithMessage("Opis nie moze byc dluzszy niz 2000 znaków.");

            RuleFor(x => x.AccommodationType)
                .NotEmpty().WithMessage("Typ zakwaterowania jest wymagany.")
                .Must(AccommodationTypeExtensions.IsValidAccommodationType)
                .WithMessage("Nieprawidlowy typ zakwaterowania.");

            RuleFor(x => x.Beds)
                .GreaterThan(0).WithMessage("Liczba lózek musi byc wieksza od zera.");

            RuleFor(x => x.MaxGuests)
                .GreaterThan(0).WithMessage("Liczba gosci musi byc wieksza od zera.")
                .GreaterThanOrEqualTo(x => x.Beds)
                .WithMessage("Liczba gosci nie moze byc mniejsza niz liczba lózek.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Kraj jest wymagany.")
                .MaximumLength(100).WithMessage("Nazwa kraju nie moze byc dluzsza niz 100 znaków.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Miasto jest wymagane.")
                .MaximumLength(100).WithMessage("Nazwa miasta nie moze byc dluzsza niz 100 znaków.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Kod pocztowy jest wymagany.")
                .Matches(@"^[0-9A-Za-z\- ]{3,10}$").WithMessage("Kod pocztowy ma nieprawidlowy format.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Ulica jest wymagana.")
                .MaximumLength(150).WithMessage("Nazwa ulicy nie moze byc dluzsza niz 150 znaków.");

            RuleFor(x => x.BuildingNumber)
                .NotEmpty().WithMessage("Numer budynku jest wymagany.")
                .MaximumLength(20).WithMessage("Numer budynku nie moze byc dluzszy niz 20 znaków.");

            RuleFor(x => x.AmountPerDay)
                .GreaterThan(0).WithMessage("Kwota za dzien musi byc wieksza od zera.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Waluta jest wymagana.")
                .Must(CurrencyExtensions.IsValidCurrency)
                .WithMessage("Nieprawidlowy kod waluty.");

            RuleFor(x => x.Photos)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lista zdjec nie moze byc pusta.")
                .Must(photos => photos.Count() == photos.Distinct().Count())
                .WithMessage("Lista zdjec nie moze zawierac duplikatów.");

            RuleForEach(x => x.Photos)
                .NotEmpty().WithMessage("Adres URL zdjecia nie moze byc pusty.");
        }
    }
}
