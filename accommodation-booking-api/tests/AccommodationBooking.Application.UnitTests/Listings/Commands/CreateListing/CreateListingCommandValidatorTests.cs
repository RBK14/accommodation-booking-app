using AccommodationBooking.Application.Listings.Commands.CreateListing;
using FluentValidation.TestHelper;

namespace AccommodationBooking.Application.UnitTests.Listings.Commands.CreateListing
{
    /// <summary>
    /// Unit tests for CreateListingCommandValidator.
    /// </summary>
    public class CreateListingCommandValidatorTests
    {
        private readonly CreateListingCommandValidator _validator;

        public CreateListingCommandValidatorTests()
        {
            _validator = new CreateListingCommandValidator();
        }

        [Fact]
        public void Validate_Should_HaveNoError_When_CommandIsValid()
        {
            var command = CreateValidCommand();

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void Validate_Should_Fail_When_TitleIsEmpty(string invalidTitle)
        {
            var command = CreateValidCommand() with { Title = invalidTitle };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Tytu? jest wymagany.");
        }

        [Fact]
        public void Validate_Should_Fail_When_TitleIsTooLong()
        {
            var command = CreateValidCommand() with { Title = new string('a', 201) };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_Should_Fail_When_BedsCountIsInvalid(int invalidBeds)
        {
            var command = CreateValidCommand() with { Beds = invalidBeds };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.Beds);
        }

        [Fact]
        public void Validate_Should_Fail_When_MaxGuestsIsLessThanBeds()
        {
            var command = CreateValidCommand() with
            {
                Beds = 4,
                MaxGuests = 3
            };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.MaxGuests)
                .WithErrorMessage("Liczba go?ci nie mo?e by? mniejsza ni? liczba ?ó?ek.");
        }

        [Fact]
        public void Validate_Should_Fail_When_AccommodationTypeIsInvalid()
        {
            var command = CreateValidCommand() with { AccommodationType = "SpaceShip" };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.AccommodationType)
                .WithErrorMessage("Nieprawid?owy typ zakwaterowania.");
        }

        [Fact]
        public void Validate_Should_Fail_When_CurrencyIsInvalid()
        {
            var command = CreateValidCommand() with { Currency = "XYZ" };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage("Nieprawid?owy kod waluty.");
        }

        [Theory]
        [InlineData("!@#")]
        [InlineData("12")]
        [InlineData("ThisIsWayTooLongPostalCode")]
        public void Validate_Should_Fail_When_PostalCodeIsInvalid(string invalidZip)
        {
            var command = CreateValidCommand() with { PostalCode = invalidZip };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.PostalCode);
        }

        [Fact]
        public void Validate_Should_Fail_When_AmountPerDayIsZeroOrNegative()
        {
            var command = CreateValidCommand() with { AmountPerDay = 0 };

            _validator.TestValidate(command)
                .ShouldHaveValidationErrorFor(x => x.AmountPerDay);
        }

        private static CreateListingCommand CreateValidCommand()
        {
            return new CreateListingCommand(
                HostProfileId: Guid.NewGuid(),
                Title: "Test Listing",
                Description: "Test Description",
                AccommodationType: "Apartment",
                Beds: 2,
                MaxGuests: 4,
                Country: "Poland",
                City: "Warsaw",
                PostalCode: "00-100",
                Street: "Test Street",
                BuildingNumber: "10A",
                AmountPerDay: 150,
                Currency: "PLN",
                Photos: new List<string> { "photo1.jpg", "photo2.jpg" }
            );
        }
    }
}
