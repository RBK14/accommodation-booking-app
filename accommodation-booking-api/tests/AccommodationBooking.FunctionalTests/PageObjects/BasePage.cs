using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; }

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfiguration.DefaultTimeout));
        }

        protected IWebElement WaitForElement(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        protected IWebElement WaitForElementToBeClickable(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        protected void WaitForElementToDisappear(By locator)
        {
            Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        protected void Click(By locator)
        {
            WaitForElementToBeClickable(locator).Click();
        }

        protected void SendKeys(By locator, string text)
        {
            var element = WaitForElement(locator);
            element.Clear();
            element.SendKeys(text);
        }

        protected string GetText(By locator)
        {
            return WaitForElement(locator).Text;
        }

        protected bool IsElementDisplayed(By locator)
        {
            try
            {
                return WaitForElement(locator).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        protected void WaitForPageLoad()
        {
            Wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        protected void TakeScreenshot(string fileName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                
                if (!Directory.Exists(screenshotPath))
                {
                    Directory.CreateDirectory(screenshotPath);
                }
                
                var fullPath = Path.Combine(screenshotPath, $"{fileName}.png");
                screenshot.SaveAsFile(fullPath);
                Console.WriteLine($"Screenshot saved: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
    }
}
