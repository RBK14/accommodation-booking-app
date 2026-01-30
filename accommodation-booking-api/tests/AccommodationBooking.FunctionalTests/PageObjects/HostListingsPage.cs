using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    public class HostListingsPage : BasePage
    {
        private const string HostListingsUrl = $"{TestConfiguration.BaseUrl}/host/listings";

        // Lokalizatory elementów
        private readonly By _addNewListingButton = By.XPath("//button[contains(., 'Dodaj og?oszenie')]");
        private readonly By _listingCards = By.XPath("//div[contains(@class, 'MuiCard-root')]");
        private readonly By _noListingsMessage = By.XPath("//*[contains(text(), 'Brak ofert') or contains(text(), 'Nie masz jeszcze')]");

        public HostListingsPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(HostListingsUrl);
            WaitForPageLoad();
        }

        public void ClickAddNewListing()
        {
            Click(_addNewListingButton);
        }

        public bool IsOnHostListingsPage()
        {
            return Driver.Url.Contains("/host/listings");
        }

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
