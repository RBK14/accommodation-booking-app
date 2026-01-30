using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the registration page.
    /// </summary>
    public class RegisterPage : BasePage
    {
        private const string RegisterUrl = TestConfiguration.RegisterUrl;

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

        /// <summary>
        /// Navigates to the registration page.
        /// </summary>
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(RegisterUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Selects the user role from the dropdown.
        /// </summary>
        public void SelectUserRole(string role)
        {
            try
            {
                Click(_userRoleSelect);
                Thread.Sleep(500);
                
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
                TakeScreenshot($"SelectUserRole_Error_{DateTime.Now:yyyyMMdd_HHmmss}");
                throw new Exception($"Cannot select user role '{role}'. Details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Enters the first name.
        /// </summary>
        public void EnterFirstName(string firstName)
        {
            SendKeys(_firstNameInput, firstName);
        }

        /// <summary>
        /// Enters the last name.
        /// </summary>
        public void EnterLastName(string lastName)
        {
            SendKeys(_lastNameInput, lastName);
        }

        /// <summary>
        /// Enters the email address.
        /// </summary>
        public void EnterEmail(string email)
        {
            SendKeys(_emailInput, email);
        }

        /// <summary>
        /// Enters the phone number.
        /// </summary>
        public void EnterPhone(string phone)
        {
            SendKeys(_phoneInput, phone);
        }

        /// <summary>
        /// Enters the password.
        /// </summary>
        public void EnterPassword(string password)
        {
            SendKeys(_passwordInput, password);
        }

        /// <summary>
        /// Enters the password confirmation.
        /// </summary>
        public void EnterConfirmPassword(string confirmPassword)
        {
            SendKeys(_confirmPasswordInput, confirmPassword);
        }

        /// <summary>
        /// Clicks the register button.
        /// </summary>
        public void ClickRegisterButton()
        {
            Click(_registerButton);
        }

        /// <summary>
        /// Performs full registration with all required fields.
        /// </summary>
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

        /// <summary>
        /// Clicks the login link.
        /// </summary>
        public void ClickLoginLink()
        {
            Click(_loginLink);
        }

        /// <summary>
        /// Clicks the show password button.
        /// </summary>
        public void ClickShowPasswordButton()
        {
            Click(_showPasswordButton);
        }

        /// <summary>
        /// Clicks the show confirm password button.
        /// </summary>
        public void ClickShowConfirmPasswordButton()
        {
            Click(_showConfirmPasswordButton);
        }

        /// <summary>
        /// Gets the password input type attribute.
        /// </summary>
        public string GetPasswordInputType()
        {
            return WaitForElement(_passwordInput).GetDomProperty("type");
        }

        /// <summary>
        /// Gets the confirm password input type attribute.
        /// </summary>
        public string GetConfirmPasswordInputType()
        {
            return WaitForElement(_confirmPasswordInput).GetDomProperty("type");
        }

        /// <summary>
        /// Checks if the register button is enabled.
        /// </summary>
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

        /// <summary>
        /// Checks if currently on the registration page.
        /// </summary>
        public bool IsOnRegisterPage()
        {
            return Driver.Url.Contains("/register");
        }

        /// <summary>
        /// Waits for the success toast notification.
        /// </summary>
        public void WaitForSuccessToast()
        {
            var toastLocator = By.XPath("//*[contains(text(), 'Rejestracja zakończona sukcesem')]");
            WaitForElement(toastLocator);
        }

        /// <summary>
        /// Waits for the error toast notification.
        /// </summary>
        public void WaitForErrorToast()
        {
            var toastLocator = By.CssSelector(".Toastify__toast--error");
            WaitForElement(toastLocator);
        }
    }
}
