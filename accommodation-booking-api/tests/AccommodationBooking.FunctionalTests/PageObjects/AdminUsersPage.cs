using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    public class AdminUsersPage : BasePage
    {
        private const string AdminUsersUrl = $"{TestConfiguration.BaseUrl}/admin/users";

        // Lokalizatory elementów
        private readonly By _usersTable = By.CssSelector("table");
        private readonly By _tableRows = By.XPath("//table//tbody//tr");
        private readonly By _successToast = By.XPath("//*[contains(text(), 'usuni?ty') or contains(text(), 'usu?')]");

        public AdminUsersPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(AdminUsersUrl);
            WaitForPageLoad();
        }

        public bool IsOnAdminUsersPage()
        {
            return Driver.Url.Contains("/admin/users");
        }

        public bool HasUsersTable()
        {
            return IsElementDisplayed(_usersTable);
        }

        public void DeleteUserByEmail(string email)
        {
            try
            {
                Thread.Sleep(2000); // Poczekaj na za?adowanie tabeli
                
                // Znajd? wiersz z u?ytkownikiem o podanym emailu
                var userRow = By.XPath($"//td[contains(text(), '{email}')]/ancestor::tr");
                var row = WaitForElement(userRow);
                
                // Znajd? wszystkie przyciski w wierszu
                var buttons = row.FindElements(By.TagName("button"));
                
                // Ostatni przycisk to przycisk usu? (kolejno??: View, Edit, Delete)
                if (buttons.Count >= 3)
                {
                    var deleteButton = buttons[buttons.Count - 1];
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", deleteButton);
                    Thread.Sleep(500);
                    deleteButton.Click();
                    
                    Thread.Sleep(1000);
                    
                    // Potwierd? usuni?cie w oknie dialogowym przegl?darki
                    try
                    {
                        var alert = Driver.SwitchTo().Alert();
                        alert.Accept();
                        Thread.Sleep(2000);
                    }
                    catch
                    {
                        // Je?li nie ma alertu, kontynuuj
                    }
                }
                else
                {
                    throw new Exception($"Nie znaleziono wystarczaj?cej liczby przycisków w wierszu u?ytkownika {email}");
                }
            }
            catch (Exception ex)
            {
                TakeScreenshot($"DeleteUser_Error_{DateTime.Now:yyyyMMdd_HHmmss}");
                throw new Exception($"Nie mo?na usun?? u?ytkownika o emailu '{email}'. Szczegó?y: {ex.Message}", ex);
            }
        }

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
                return true; // Je?li nie mo?na znale??, to znaczy ?e zosta? usuni?ty
            }
        }

        public bool IsSuccessToastDisplayed()
        {
            return IsElementDisplayed(_successToast);
        }

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
