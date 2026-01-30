using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    public class RegisterPage : BasePage
    {
        private const string RegisterUrl = TestConfiguration.RegisterUrl;

        // Lokalizatory elementów - zaktualizowane dla Material-UI
        private readonly By _userRoleSelect = By.XPath("//label[text()='Typ konta']/following-sibling::div");
        private readonly By _userRoleGuestOption = By.XPath("//li[@data-value='Guest']");
        private readonly By _userRoleHostOption = By.XPath("//li[@data-value='Host']");
        private readonly By _firstNameInput = By.XPath("//input[@name='firstName']");
        private readonly By _lastNameInput = By.XPath("//input[@name='lastName']");
        private readonly By _emailInput = By.XPath("//input[@name='email']");
        private readonly By _phoneInput = By.XPath("//input[@name='phone']");
        private readonly By _passwordInput = By.XPath("//input[@name='password']");
        private readonly By _confirmPasswordInput = By.XPath("//input[@name='confirmPassword']");
        private readonly By _registerButton = By.XPath("//button[contains(text(), 'Zarejestruj się') or contains(text(), 'Rejestracja...')]");
        private readonly By _loginLink = By.XPath("//span[contains(text(), 'Zaloguj się')]");
        private readonly By _showPasswordButton = By.XPath("(//button[@aria-label='toggle password visibility'])[1]");
        private readonly By _showConfirmPasswordButton = By.XPath("(//button[@aria-label='toggle confirm password visibility'])[1]");

        public RegisterPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(RegisterUrl);
            WaitForPageLoad();
        }

        public void SelectUserRole(string role)
        {
            try
            {
                // Kliknij w Select aby otworzyć menu
                Click(_userRoleSelect);
                Thread.Sleep(500); // Czekaj na otwarcie menu
                
                if (role.Equals("Guest", StringComparison.OrdinalIgnoreCase))
                {
                    Click(_userRoleGuestOption);
                }
                else if (role.Equals("Host", StringComparison.OrdinalIgnoreCase))
                {
                    Click(_userRoleHostOption);
                }
            }
            catch (Exception ex)
            {
                // Zapisz screenshot dla debugowania
                TakeScreenshot($"SelectUserRole_Error_{DateTime.Now:yyyyMMdd_HHmmss}");
                throw new Exception($"Nie można wybrać roli użytkownika '{role}'. Szczegóły: {ex.Message}", ex);
            }
        }

        public void EnterFirstName(string firstName)
        {
            SendKeys(_firstNameInput, firstName);
        }

        public void EnterLastName(string lastName)
        {
            SendKeys(_lastNameInput, lastName);
        }

        public void EnterEmail(string email)
        {
            SendKeys(_emailInput, email);
        }

        public void EnterPhone(string phone)
        {
            SendKeys(_phoneInput, phone);
        }

        public void EnterPassword(string password)
        {
            SendKeys(_passwordInput, password);
        }

        public void EnterConfirmPassword(string confirmPassword)
        {
            SendKeys(_confirmPasswordInput, confirmPassword);
        }

        public void ClickRegisterButton()
        {
            Click(_registerButton);
        }

        public void Register(string userRole, string firstName, string lastName, string email, 
            string phone, string password, string confirmPassword)
        {
            SelectUserRole(userRole);
            EnterFirstName(firstName);
            EnterLastName(lastName);
            EnterEmail(email);
            EnterPhone(phone);
            EnterPassword(password);
            EnterConfirmPassword(confirmPassword);
            ClickRegisterButton();
        }

        public void ClickLoginLink()
        {
            Click(_loginLink);
        }

        public void ClickShowPasswordButton()
        {
            Click(_showPasswordButton);
        }

        public void ClickShowConfirmPasswordButton()
        {
            Click(_showConfirmPasswordButton);
        }

        public string GetPasswordInputType()
        {
            return WaitForElement(_passwordInput).GetDomProperty("type");
        }

        public string GetConfirmPasswordInputType()
        {
            return WaitForElement(_confirmPasswordInput).GetDomProperty("type");
        }

        public bool IsRegisterButtonEnabled()
        {
            try
            {
                return WaitForElement(_registerButton).Enabled;
            }
            catch
            {
                return false;
            }
        }

        public bool IsOnRegisterPage()
        {
            return Driver.Url.Contains("/register");
        }

        public void WaitForSuccessToast()
        {
            var toastLocator = By.XPath("//*[contains(text(), 'Rejestracja zakończona sukcesem')]");
            WaitForElement(toastLocator);
        }

        public void WaitForErrorToast()
        {
            var toastLocator = By.CssSelector(".Toastify__toast--error");
            WaitForElement(toastLocator);
        }
    }
}
