using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the new listing creation page.
    /// </summary>
    public class HostNewListingPage : BasePage
    {
        private const string NewListingUrl = $"{TestConfiguration.BaseUrl}/host/new-listing";

        private readonly By _titleInput = By.XPath("//input[@name='title']");
        private readonly By _descriptionInput = By.XPath("//textarea[@name='description']");
        private readonly By _accommodationTypeSelect = By.XPath("//label[text()='Typ zakwaterowania']/following-sibling::div");
        private readonly By _bedsInput = By.XPath("//input[@name='beds']");
        private readonly By _maxGuestsInput = By.XPath("//input[@name='maxGuests']");
        private readonly By _countryInput = By.XPath("//input[@name='country']");
        private readonly By _cityInput = By.XPath("//input[@name='city']");
        private readonly By _postalCodeInput = By.XPath("//input[@name='postalCode']");
        private readonly By _streetInput = By.XPath("//input[@name='street']");
        private readonly By _buildingNumberInput = By.XPath("//input[@name='buildingNumber']");
        private readonly By _amountPerDayInput = By.XPath("//input[@name='amountPerDay']");
        private readonly By _currencySelect = By.XPath("//label[text()='Waluta']/following-sibling::div");
        private readonly By _addImageButton = By.XPath("//button[contains(., 'Dodaj')]");
        private readonly By _saveButton = By.XPath("//button[contains(., 'Zapisz')]");
        private readonly By _cancelButton = By.XPath("//button[contains(., 'Anuluj')]");
        private readonly By _errorAlert = By.CssSelector(".MuiAlert-standardError");
        private readonly By _imagesList = By.CssSelector("ul[class*='MuiImageList-root']");

        public HostNewListingPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the new listing page.
        /// </summary>
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(NewListingUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Enters the listing title.
        /// </summary>
        public void EnterTitle(string title)
        {
            SendKeys(_titleInput, title);
        }

        /// <summary>
        /// Enters the listing description.
        /// </summary>
        public void EnterDescription(string description)
        {
            SendKeys(_descriptionInput, description);
        }

        /// <summary>
        /// Selects the accommodation type.
        /// </summary>
        public void SelectAccommodationType(string type)
        {
            Click(_accommodationTypeSelect);
            Thread.Sleep(500);
            var option = By.XPath($"//li[@data-value='{type}']");
            Click(option);
        }

        /// <summary>
        /// Enters the number of beds.
        /// </summary>
        public void EnterBeds(int beds)
        {
            SendKeys(_bedsInput, beds.ToString());
        }

        /// <summary>
        /// Enters the maximum number of guests.
        /// </summary>
        public void EnterMaxGuests(int maxGuests)
        {
            SendKeys(_maxGuestsInput, maxGuests.ToString());
        }

        /// <summary>
        /// Enters the country.
        /// </summary>
        public void EnterCountry(string country)
        {
            SendKeys(_countryInput, country);
        }

        /// <summary>
        /// Enters the city.
        /// </summary>
        public void EnterCity(string city)
        {
            SendKeys(_cityInput, city);
        }

        /// <summary>
        /// Enters the postal code.
        /// </summary>
        public void EnterPostalCode(string postalCode)
        {
            SendKeys(_postalCodeInput, postalCode);
        }

        /// <summary>
        /// Enters the street name.
        /// </summary>
        public void EnterStreet(string street)
        {
            SendKeys(_streetInput, street);
        }

        /// <summary>
        /// Enters the building number.
        /// </summary>
        public void EnterBuildingNumber(string buildingNumber)
        {
            SendKeys(_buildingNumberInput, buildingNumber);
        }

        /// <summary>
        /// Enters the price per day.
        /// </summary>
        public void EnterAmountPerDay(decimal amount)
        {
            SendKeys(_amountPerDayInput, amount.ToString("0.00"));
        }

        /// <summary>
        /// Selects the currency.
        /// </summary>
        public void SelectCurrency(string currency)
        {
            Click(_currencySelect);
            Thread.Sleep(500);
            var option = By.XPath($"//li[@data-value='{currency}']");
            Click(option);
        }

        /// <summary>
        /// Adds an image URL to the listing.
        /// </summary>
        public void AddImage(string imageUrl)
        {
            try
            {
                var button = WaitForElement(_addImageButton);
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", button);
                Thread.Sleep(500);
                
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", button);
                Thread.Sleep(500);
                
                var alert = Driver.SwitchTo().Alert();
                alert.SendKeys(imageUrl);
                alert.Accept();
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                TakeScreenshot($"AddImage_Error_{DateTime.Now:yyyyMMdd_HHmmss}");
                throw new Exception($"Cannot add image. Details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clicks the save button.
        /// </summary>
        public void ClickSaveButton()
        {
            var button = WaitForElement(_saveButton);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", button);
            Thread.Sleep(500);
            Click(_saveButton);
        }

        /// <summary>
        /// Clicks the cancel button.
        /// </summary>
        public void ClickCancelButton()
        {
            Click(_cancelButton);
        }

        /// <summary>
        /// Checks if an error alert is displayed.
        /// </summary>
        public bool IsErrorAlertDisplayed()
        {
            return IsElementDisplayed(_errorAlert);
        }

        /// <summary>
        /// Gets the error message text.
        /// </summary>
        public string GetErrorMessage()
        {
            return GetText(_errorAlert);
        }

        /// <summary>
        /// Checks if currently on the new listing page.
        /// </summary>
        public bool IsOnNewListingPage()
        {
            return Driver.Url.Contains("/host/new-listing");
        }

        /// <summary>
        /// Checks if there are any images added.
        /// </summary>
        public bool HasImages()
        {
            try
            {
                var imageList = WaitForElement(_imagesList);
                return imageList.FindElements(By.TagName("li")).Count > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a complete listing with all required fields.
        /// </summary>
        public void CreateListing(
            string title,
            string description,
            string accommodationType,
            int beds,
            int maxGuests,
            string country,
            string city,
            string postalCode,
            string street,
            string buildingNumber,
            decimal amountPerDay,
            string currency,
            string imageUrl)
        {
            EnterTitle(title);
            EnterDescription(description);
            SelectAccommodationType(accommodationType);
            EnterBeds(beds);
            EnterMaxGuests(maxGuests);
            EnterCountry(country);
            EnterCity(city);
            EnterPostalCode(postalCode);
            EnterStreet(street);
            EnterBuildingNumber(buildingNumber);
            EnterAmountPerDay(amountPerDay);
            SelectCurrency(currency);
            AddImage(imageUrl);
            ClickSaveButton();
        }
    }
}
