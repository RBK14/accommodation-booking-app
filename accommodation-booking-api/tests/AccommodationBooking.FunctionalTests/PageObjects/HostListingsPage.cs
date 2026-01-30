using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the host listings page.
    /// </summary>
    public class HostListingsPage : BasePage
    {
        private const string HostListingsUrl = $"{TestConfiguration.BaseUrl}/host/listings";

        private readonly By _addNewListingButton = By.XPath("//button[contains(., 'Dodaj og?oszenie')]");
        private readonly By _listingCards = By.XPath("//div[contains(@class, 'MuiCard-root')]");
        private readonly By _noListingsMessage = By.XPath("//*[contains(text(), 'Brak ofert') or contains(text(), 'Nie masz jeszcze')]");

        public HostListingsPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the host listings page.
        /// </summary>
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(HostListingsUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Clicks the add new listing button.
        /// </summary>
        public void ClickAddNewListing()
        {
            Click(_addNewListingButton);
        }

        /// <summary>
        /// Checks if currently on the host listings page.
        /// </summary>
        public bool IsOnHostListingsPage()
        {
            return Driver.Url.Contains("/host/listings");
        }

        /// <summary>
        /// Checks if there are any listings displayed.
        /// </summary>
        public bool HasListings()
        {
            try
            {
                var cards = Driver.FindElements(_listingCards);
                return cards.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of listings displayed.
        /// </summary>
        public int GetListingsCount()
        {
            try
            {
                Thread.Sleep(1000);
                return Driver.FindElements(_listingCards).Count;
            }
            catch
            {
                return 0;
            }
        }
    }
}
