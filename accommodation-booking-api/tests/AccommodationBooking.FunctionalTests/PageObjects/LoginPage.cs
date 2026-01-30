using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    public class LoginPage : BasePage
    {
        private const string LoginUrl = TestConfiguration.LoginUrl;

        // Lokalizatory elementów - zaktualizowane dla Material-UI
        private readonly By _emailInput = By.XPath("//input[@type='email']");
        private readonly By _passwordInput = By.XPath("//label[contains(text(), 'Hasło')]/following-sibling::div//input");
        private readonly By _loginButton = By.XPath("//button[@type='submit']");
        private readonly By _registerLink = By.XPath("//span[contains(text(), 'Zarejestruj się')]");
        private readonly By _errorAlert = By.CssSelector(".MuiAlert-standardError");
        private readonly By _showPasswordButton = By.CssSelector("button[aria-label='toggle password visibility']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            WaitForPageLoad();
        }

        public void EnterEmail(string email)
        {
            SendKeys(_emailInput, email);
        }

        public void EnterPassword(string password)
        {
            SendKeys(_passwordInput, password);
        }

        public void ClickLoginButton()
        {
            Click(_loginButton);
        }

        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLoginButton();
        }

        public bool IsErrorAlertDisplayed()
        {
            return IsElementDisplayed(_errorAlert);
        }

        public string GetErrorMessage()
        {
            return GetText(_errorAlert);
        }

        public void ClickRegisterLink()
        {
            Click(_registerLink);
        }

        public void ClickShowPasswordButton()
        {
            Click(_showPasswordButton);
        }

        public string GetPasswordInputType()
        {
            return WaitForElement(_passwordInput).GetDomProperty("type");
        }

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

        public bool IsOnLoginPage()
        {
            return Driver.Url.Contains("/login");
        }
    }
}
