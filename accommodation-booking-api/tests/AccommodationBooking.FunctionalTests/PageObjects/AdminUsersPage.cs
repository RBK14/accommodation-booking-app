using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Page object for the admin users management page.
    /// </summary>
    public class AdminUsersPage : BasePage
    {
        private const string AdminUsersUrl = $"{TestConfiguration.BaseUrl}/admin/users";

        private readonly By _usersTable = By.CssSelector("table");
        private readonly By _tableRows = By.XPath("//table//tbody//tr");
        private readonly By _successToast = By.XPath("//*[contains(text(), 'usuni?ty') or contains(text(), 'usu?')]");

        public AdminUsersPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the admin users page.
        /// </summary>
        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(AdminUsersUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Checks if currently on the admin users page.
        /// </summary>
        public bool IsOnAdminUsersPage()
        {
            return Driver.Url.Contains("/admin/users");
        }

        /// <summary>
        /// Checks if the users table is displayed.
        /// </summary>
        public bool HasUsersTable()
        {
            return IsElementDisplayed(_usersTable);
        }

        /// <summary>
        /// Deletes a user by their email address.
        /// </summary>
        public void DeleteUserByEmail(string email)
        {
            try
            {
                Thread.Sleep(2000);

                var userRow = By.XPath($"//td[contains(text(), '{email}')]/ancestor::tr");
                var row = WaitForElement(userRow);

                var buttons = row.FindElements(By.TagName("button"));

                if (buttons.Count >= 3)
                {
                    var deleteButton = buttons[buttons.Count - 1];
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", deleteButton);
                    Thread.Sleep(500);
                    deleteButton.Click();

                    Thread.Sleep(1000);

                    try
                    {
                        var alert = Driver.SwitchTo().Alert();
                        alert.Accept();
                        Thread.Sleep(2000);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    throw new Exception($"Not enough buttons found in user row for {email}");
                }
            }
            catch (Exception ex)
            {
                TakeScreenshot($"DeleteUser_Error_{DateTime.Now:yyyyMMdd_HHmmss}");
                throw new Exception($"Cannot delete user with email '{email}'. Details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a user was deleted.
        /// </summary>
        public bool IsUserDeleted(string email)
        {
            try
            {
                Thread.Sleep(2000);
                var userRow = Driver.FindElements(By.XPath($"//td[contains(text(), '{email}')]"));
                return userRow.Count == 0;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if a success toast is displayed.
        /// </summary>
        public bool IsSuccessToastDisplayed()
        {
            return IsElementDisplayed(_successToast);
        }

        /// <summary>
        /// Gets the total number of users in the table.
        /// </summary>
        public int GetUsersCount()
        {
            try
            {
                Thread.Sleep(2000);
                return Driver.FindElements(_tableRows).Count;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Checks if a user exists by their email.
        /// </summary>
        public bool UserExistsByEmail(string email)
        {
            try
            {
                Thread.Sleep(2000);
                var userCells = Driver.FindElements(By.XPath($"//td[contains(text(), '{email}')]"));
                return userCells.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
