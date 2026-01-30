using AccommodationBooking.FunctionalTests.Configuration;

namespace AccommodationBooking.FunctionalTests
{
    /// <summary>
    /// End-to-end tests for authentication and authorization scenarios.
    /// </summary>
    public class AuthorizationTests : BaseTest
    {
        [Fact]
        public void Register_WithDifferentRoles_ShouldCreateDifferentAccounts()
        {
            var guestEmail = GenerateUniqueEmail("guest");
            var hostEmail = GenerateUniqueEmail("host");

            RegisterNewGuest(email: guestEmail);
            Wait(2000);

            Assert.True(LoginPage.IsOnLoginPage());

            RegisterNewHost(email: hostEmail);
            Wait(2000);

            Assert.True(LoginPage.IsOnLoginPage());
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldRedirectToHomePage()
        {
            var email = TestConfiguration.TestData.Guest.Email;
            var password = TestConfiguration.TestData.Guest.Password;

            LoginPage.NavigateTo();
            LoginPage.Login(email, password);
            Wait(2000);

            Assert.False(LoginPage.IsOnLoginPage());
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldShowErrorMessage()
        {
            LoginPage.NavigateTo();
            var email = "invalid@test.com";
            var password = "WrongPassword123!";

            LoginPage.Login(email, password);
            Wait(1000);

            Assert.True(LoginPage.IsErrorAlertDisplayed());
        }

        [Fact]
        public void Register_ThenLogin_ShouldSucceed()
        {
            var email = GenerateUniqueEmail("journey");
            var password = "Test123!";

            RegisterNewGuest(email: email, password: password);
            Wait(2000);

            Assert.True(LoginPage.IsOnLoginPage());

            LoginPage.Login(email, password);
            Wait(2000);

            Assert.False(LoginPage.IsOnLoginPage());
        }
    }
}
