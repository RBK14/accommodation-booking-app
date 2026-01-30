using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    public class GuestListingsPage : BasePage
    {
        private const string ListingsUrl = $"{TestConfiguration.BaseUrl}/";

        // Lokalizatory elementów
        private readonly By _listingCards = By.XPath("//div[contains(@class, 'MuiCard-root')]");
        private readonly By _viewButton = By.XPath("//button[contains(., 'Zobacz szczegó?y')]");

        public GuestListingsPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(ListingsUrl);
            WaitForPageLoad();
        }

        public void ClickFirstListingViewButton()
        {
            Thread.Sleep(2000); // Poczekaj na za?adowanie ofert
            var buttons = Driver.FindElements(_viewButton);
            if (buttons.Count > 0)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", buttons[0]);
                Thread.Sleep(500);
                buttons[0].Click();
            }
        }

        public string GetFirstListingId()
        {
            Thread.Sleep(2000);
            var buttons = Driver.FindElements(_viewButton);
            if (buttons.Count > 0)
            {
                // Wyci?gnij ID z onclick lub href
                var onClick = buttons[0].GetAttribute("onclick");
                // Tutaj zaimplementuj logik? wyci?gania ID
                return "1"; // Placeholder - w prawdziwej implementacji trzeba wyci?gn?? ID
            }
            return null;
        }

        public bool HasListings()
        {
            try
            {
                Thread.Sleep(2000);
                return Driver.FindElements(_listingCards).Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
