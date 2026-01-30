using AccommodationBooking.FunctionalTests.Configuration;
using AccommodationBooking.FunctionalTests.Helpers;
using AccommodationBooking.FunctionalTests.PageObjects;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests
{
    /// <summary>
    /// Base class for functional tests providing common functionality.
    /// </summary>
    public abstract class BaseTest : IDisposable
    {
        protected IWebDriver Driver { get; }
        protected LoginPage LoginPage { get; }
        protected RegisterPage RegisterPage { get; }
        protected HostNewListingPage HostNewListingPage { get; }
        protected HostListingsPage HostListingsPage { get; }
        protected ReservationPage ReservationPage { get; }
        protected GuestListingsPage GuestListingsPage { get; }
        protected AdminUsersPage AdminUsersPage { get; }

        protected BaseTest()
        {
            Driver = WebDriverFactory.CreateChromeDriver();
            LoginPage = new LoginPage(Driver);
            RegisterPage = new RegisterPage(Driver);
            HostNewListingPage = new HostNewListingPage(Driver);
            HostListingsPage = new HostListingsPage(Driver);
            ReservationPage = new ReservationPage(Driver);
            GuestListingsPage = new GuestListingsPage(Driver);
            AdminUsersPage = new AdminUsersPage(Driver);
        }

        /// <summary>
        /// Generates a unique email for tests.
        /// </summary>
        protected string GenerateUniqueEmail(string prefix = "test")
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return $"{prefix}{timestamp}@test.com";
        }

        /// <summary>
        /// Logs in as a guest user.
        /// </summary>
        protected void LoginAsGuest()
        {
            LoginPage.NavigateTo();
            LoginPage.Login(
                TestConfiguration.TestData.Guest.Email,
                TestConfiguration.TestData.Guest.Password
            );
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Logs in as a host user.
        /// </summary>
        protected void LoginAsHost()
        {
            LoginPage.NavigateTo();
            LoginPage.Login(
                TestConfiguration.TestData.Host.Email,
                TestConfiguration.TestData.Host.Password
            );
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Logs in as an admin user.
        /// </summary>
        protected void LoginAsAdmin()
        {
            LoginPage.NavigateTo();
            LoginPage.Login(
                TestConfiguration.TestData.Admin.Email,
                TestConfiguration.TestData.Admin.Password
            );
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Registers a new guest user.
        /// </summary>
        protected void RegisterNewGuest(
            string? email = null,
            string firstName = "Jan",
            string lastName = "Kowalski",
            string phone = "+48123456789",
            string password = "Test123!")
        {
            RegisterPage.NavigateTo();
            RegisterPage.Register(
                userRole: "Guest",
                firstName: firstName,
                lastName: lastName,
                email: email ?? GenerateUniqueEmail("guest"),
                phone: phone,
                password: password,
                confirmPassword: password
            );
        }

        /// <summary>
        /// Registers a new host user.
        /// </summary>
        protected void RegisterNewHost(
            string? email = null,
            string firstName = "Anna",
            string lastName = "Nowak",
            string phone = "+48987654321",
            string password = "Test123!")
        {
            RegisterPage.NavigateTo();
            RegisterPage.Register(
                userRole: "Host",
                firstName: firstName,
                lastName: lastName,
                email: email ?? GenerateUniqueEmail("host"),
                phone: phone,
                password: password,
                confirmPassword: password
            );
        }

        /// <summary>
        /// Waits for the specified amount of time.
        /// </summary>
        protected void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public virtual void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}
