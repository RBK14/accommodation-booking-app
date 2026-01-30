using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the login page.
    /// </summary>
    public class LoginPage : BasePage
    {
        private const string LoginUrl = TestConfiguration.LoginUrl;

        private readonly By _emailInput = By.XPath("//input[@type='email']");
        private readonly By _passwordInput = By.XPath("//label[contains(text(), 'Has?o')]/following-sibling::div//input");
        private readonly By _loginButton = By.XPath("//button[@type='submit']");
        private readonly By _registerLink = By.XPath("//span[contains(text(), 'Zarejestruj si?')]");
        private readonly By _errorAlert = By.CssSelector(".MuiAlert-standardError");
        private readonly By _showPasswordButton = By.CssSelector("button[aria-label='toggle password visibility']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the login page.
        /// </summary>
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Enters email in the email field.
        /// </summary>
        public void EnterEmail(string email)
        {
            SendKeys(_emailInput, email);
        }

        /// <summary>
        /// Enters password in the password field.
        /// </summary>
        public void EnterPassword(string password)
        {
            SendKeys(_passwordInput, password);
        }

        /// <summary>
        /// Clicks the login button.
        /// </summary>
        public void ClickLoginButton()
        {
            Click(_loginButton);
        }

        /// <summary>
        /// Performs login with provided credentials.
        /// </summary>
        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLoginButton();
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
        /// Clicks the register link.
        /// </summary>
        public void ClickRegisterLink()
        {
            Click(_registerLink);
        }

        /// <summary>
        /// Clicks the show password button.
        /// </summary>
        public void ClickShowPasswordButton()
        {
            Click(_showPasswordButton);
        }

        /// <summary>
        /// Gets the password input type attribute.
        /// </summary>
        public string GetPasswordInputType()
        {
            return WaitForElement(_passwordInput).GetDomProperty("type");
        }

        /// <summary>
        /// Checks if the login button is enabled.
        /// </summary>
        public bool IsLoginButtonEnabled()
        {
            try
            {
                return WaitForElement(_loginButton).Enabled;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if currently on the login page.
        /// </summary>
        public bool IsOnLoginPage()
        {
            return Driver.Url.Contains("/login");
        }
    }
}
