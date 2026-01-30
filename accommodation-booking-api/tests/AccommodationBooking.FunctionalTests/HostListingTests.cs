using AccommodationBooking.FunctionalTests.Configuration;

namespace AccommodationBooking.FunctionalTests
{
    /// <summary>
    /// Functional tests for host listing management operations.
    /// </summary>
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

            // Assert - should be on the new listing page
            Assert.True(HostNewListingPage.IsOnNewListingPage());

            // Act - fill the form
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            HostNewListingPage.CreateListing(
                title: $"Test Apartment {timestamp}",
                description: "Beautiful apartment in the city center with a view of the park",
                accommodationType: "Apartment",
                beds: 2,
                maxGuests: 4,
                country: "Poland",
                city: "Warsaw",
                postalCode: "00-001",
                street: "Test Street",
                buildingNumber: "1",
                amountPerDay: 250.00m,
                currency: "PLN",
                imageUrl: "https://picsum.photos/800/600"
            );

            Wait(3000);

            // Assert - should redirect to host listings page
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

            // Act - fill only some fields
            HostNewListingPage.EnterTitle("Incomplete Test");
            HostNewListingPage.ClickSaveButton();
            Wait(1000);

            // Assert - should display an error
            Assert.True(HostNewListingPage.IsErrorAlertDisplayed());
            var errorMessage = HostNewListingPage.GetErrorMessage().ToLower();
            Assert.True(errorMessage.Contains("wymagane") || errorMessage.Contains("wszystkie"));
        }
    }
}
