using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AccommodationBooking.FunctionalTests.PageObjects
{
    /// <summary>
    /// Base page object providing common Selenium operations.
    /// </summary>
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; }

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfiguration.DefaultTimeout));
        }

        /// <summary>
        /// Waits for an element to be visible.
        /// </summary>
        protected IWebElement WaitForElement(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>
        /// Waits for an element to be clickable.
        /// </summary>
        protected IWebElement WaitForElementToBeClickable(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// Waits for an element to disappear from the page.
        /// </summary>
        protected void WaitForElementToDisappear(By locator)
        {
            Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        /// <summary>
        /// Clicks on an element.
        /// </summary>
        protected void Click(By locator)
        {
            WaitForElementToBeClickable(locator).Click();
        }

        /// <summary>
        /// Clears and sends keys to an input element.
        /// </summary>
        protected void SendKeys(By locator, string text)
        {
            var element = WaitForElement(locator);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Gets the text content of an element.
        /// </summary>
        protected string GetText(By locator)
        {
            return WaitForElement(locator).Text;
        }

        /// <summary>
        /// Checks if an element is displayed on the page.
        /// </summary>
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

        /// <summary>
        /// Waits for the page to fully load.
        /// </summary>
        protected void WaitForPageLoad()
        {
            Wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        /// <summary>
        /// Takes a screenshot and saves it to the Screenshots folder.
        /// </summary>
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
