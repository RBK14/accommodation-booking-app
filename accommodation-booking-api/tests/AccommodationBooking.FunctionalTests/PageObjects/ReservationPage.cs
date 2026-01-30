using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the reservation page.
    /// </summary>
    public class ReservationPage : BasePage
    {
        private readonly By _confirmButton = By.XPath("//button[contains(., 'Potwierdź rezerwację')]");
        private readonly By _totalPriceLabel = By.XPath("//*[contains(text(), 'Całkowita cena:')]");
        private readonly By _listingTitle = By.XPath("//h6");
        private readonly By _successToast = By.CssSelector(".Toastify__toast--success");
        private readonly By _errorToast = By.CssSelector(".Toastify__toast--error");
        private readonly By _calendarIcon = By.XPath("//button[contains(@class, 'MuiIconButton-root') and descendant::*[contains(@data-testid, 'CalendarIcon')]]");
        private readonly By _availableDays = By.XPath("//button[@role='gridcell' and not(contains(@class, 'Mui-disabled')) and not(@disabled)]");
        private readonly By _nextMonthButton = By.XPath("//button[contains(@aria-label, 'Next month')]");

        public ReservationPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the reservation page for a specific listing.
        /// </summary>
        public void NavigateToReservation(string listingId)
        {
            var url = $"{TestConfiguration.BaseUrl}/reservation/{listingId}";
            Driver.Navigate().GoToUrl(url);
            WaitForPageLoad();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Selects a check-in date from the calendar.
        /// </summary>
        public void SelectCheckInDate()
        {
            Thread.Sleep(1000);
            
            var calendarIcons = Driver.FindElements(_calendarIcon);
            if (calendarIcons.Count < 1) return;
            
            calendarIcons[0].Click();
            Thread.Sleep(1500);
            
            var availableDays = Driver.FindElements(_availableDays);
            
            if (availableDays.Count < 2)
            {
                var nextMonth = Driver.FindElements(_nextMonthButton);
                if (nextMonth.Count > 0)
                {
                    nextMonth[0].Click();
                    Thread.Sleep(1000);
                    availableDays = Driver.FindElements(_availableDays);
                }
            }

            if (availableDays.Count >= 2)
            {
                new OpenQA.Selenium.Interactions.Actions(Driver)
                    .MoveToElement(availableDays[1])
                    .Click()
                    .Perform();
            }
            else if (availableDays.Count == 1)
            {
                new OpenQA.Selenium.Interactions.Actions(Driver)
                    .MoveToElement(availableDays[0])
                    .Click()
                    .Perform();
            }
            
            Thread.Sleep(1500);
        }

        /// <summary>
        /// Selects a check-out date from the calendar.
        /// </summary>
        public void SelectCheckOutDate()
        {
            Thread.Sleep(1000);
            
            var calendarIcons = Driver.FindElements(_calendarIcon);
            if (calendarIcons.Count < 2) return;
            
            calendarIcons[1].Click();
            Thread.Sleep(1500);
            
            var availableDays = Driver.FindElements(_availableDays);
            
            if (availableDays.Count == 0)
            {
                var nextMonth = Driver.FindElements(_nextMonthButton);
                if (nextMonth.Count > 0)
                {
                    nextMonth[0].Click();
                    Thread.Sleep(1000);
                    availableDays = Driver.FindElements(_availableDays);
                }
            }

            if (availableDays.Count >= 1)
            {
                new OpenQA.Selenium.Interactions.Actions(Driver)
                    .MoveToElement(availableDays[0])
                    .Click()
                    .Perform();
            }
            
            Thread.Sleep(1500);
        }

        /// <summary>
        /// Clicks the confirm reservation button.
        /// </summary>
        public void ClickConfirmButton()
        {
            var button = WaitForElement(_confirmButton);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", button);
            Thread.Sleep(500);
            button.Click();
        }

        /// <summary>
        /// Checks if the confirm button is enabled.
        /// </summary>
        public bool IsConfirmButtonEnabled()
        {
            try
            {
                var button = WaitForElement(_confirmButton);
                var isDisabled = button.GetDomProperty("disabled");
                return isDisabled == null || isDisabled.ToString().ToLower() == "false" || isDisabled.ToString() == "";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the total price is displayed.
        /// </summary>
        public bool HasTotalPrice()
        {
            try
            {
                Thread.Sleep(1000);
                var elements = Driver.FindElements(_totalPriceLabel);
                return elements.Count > 0 && elements[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a success toast notification is displayed.
        /// </summary>
        public bool IsSuccessToastDisplayed()
        {
            try
            {
                Thread.Sleep(1500);
                var elements = Driver.FindElements(_successToast);
                return elements.Count > 0 && elements[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the error message from toast notification.
        /// </summary>
        public string GetErrorMessage()
        {
            try
            {
                var elements = Driver.FindElements(_errorToast);
                return elements.Count > 0 && elements[0].Displayed ? elements[0].Text : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks if currently on the reservation page.
        /// </summary>
        public bool IsOnReservationPage() => Driver.Url.Contains("/reservation/");

        /// <summary>
        /// Gets the listing title from the page.
        /// </summary>
        public string GetListingTitle()
        {
            try { return GetText(_listingTitle); }
            catch { return string.Empty; }
        }
    }
}
