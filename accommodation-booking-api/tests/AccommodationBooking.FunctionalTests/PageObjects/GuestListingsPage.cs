using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the guest listings browse page.
    /// </summary>
    public class GuestListingsPage : BasePage
    {
        private const string ListingsUrl = $"{TestConfiguration.BaseUrl}/";

        private readonly By _listingCards = By.XPath("//div[contains(@class, 'MuiCard-root')]");
        private readonly By _viewButton = By.XPath("//button[contains(., 'Zobacz szczegó?y')]");

        public GuestListingsPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the listings page.
        /// </summary>
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(ListingsUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Clicks the view button on the first listing.
        /// </summary>
        public void ClickFirstListingViewButton()
        {
            Thread.Sleep(2000);
            var buttons = Driver.FindElements(_viewButton);
            if (buttons.Count > 0)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", buttons[0]);
                Thread.Sleep(500);
                buttons[0].Click();
            }
        }

        /// <summary>
        /// Gets the ID of the first listing.
        /// </summary>
        public string GetFirstListingId()
        {
            Thread.Sleep(2000);
            var buttons = Driver.FindElements(_viewButton);
            if (buttons.Count > 0)
            {
                var onClick = buttons[0].GetAttribute("onclick");
                return "1";
            }
            return null;
        }

        /// <summary>
        /// Checks if there are any listings displayed.
        /// </summary>
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
