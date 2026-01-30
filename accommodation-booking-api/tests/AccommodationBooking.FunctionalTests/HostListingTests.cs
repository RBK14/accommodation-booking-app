using AccommodationBooking.FunctionalTests.Configuration;

namespace AccommodationBooking.FunctionalTests
{
    public class HostListingTests : BaseTest
    {
        [Fact]
        public void CreateListing_WithValidData_ShouldSucceed()
        {
            // Arrange
            LoginAsHost();
            Wait(2000);
            
            HostNewListingPage.NavigateTo();
            Wait(1000);

            // Assert - powinien być na stronie tworzenia oferty
            Assert.True(HostNewListingPage.IsOnNewListingPage());

            // Act - wypełnij formularz
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            HostNewListingPage.CreateListing(
                title: $"Apartament testowy {timestamp}",
                description: "Piękny apartament w centrum miasta z widokiem na park",
                accommodationType: "Apartment",
                beds: 2,
                maxGuests: 4,
                country: "Polska",
                city: "Warszawa",
                postalCode: "00-001",
                street: "Testowa",
                buildingNumber: "1",
                amountPerDay: 250.00m,
                currency: "PLN",
                imageUrl: "https://picsum.photos/800/600"
            );

            Wait(3000);

            // Assert - powinien przekierować do listy ofert
            Assert.True(HostListingsPage.IsOnHostListingsPage());
        }

        [Fact]
        public void CreateListing_WithoutRequiredFields_ShouldShowError()
        {
            // Arrange
            LoginAsHost();
            Wait(2000);
            HostNewListingPage.NavigateTo();
            Wait(1000);

            // Act - wypełnij tylko część pól
            HostNewListingPage.EnterTitle("Test niepełny");
            HostNewListingPage.ClickSaveButton();
            Wait(1000);

            // Assert - powinien wyświetlić błąd
            Assert.True(HostNewListingPage.IsErrorAlertDisplayed());
            var errorMessage = HostNewListingPage.GetErrorMessage().ToLower();
            Assert.True(errorMessage.Contains("wymagane") || errorMessage.Contains("wszystkie"));
        }
    }
}
