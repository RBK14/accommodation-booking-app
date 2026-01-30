using AccommodationBooking.FunctionalTests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AccommodationBooking.FunctionalTests.Helpers
{
    /// <summary>
    /// Factory for creating WebDriver instances.
    /// </summary>
    public static class WebDriverFactory
    {
        /// <summary>
        /// Creates a Chrome WebDriver with configured options.
        /// </summary>
        public static IWebDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();
            
            if (TestConfiguration.BrowserOptions.Maximized)
                options.AddArgument("--start-maximized");
            
            if (TestConfiguration.BrowserOptions.DisableNotifications)
            {
                options.AddArgument("--disable-notifications");
                options.AddArgument("--disable-popup-blocking");
            }
            
            options.AddArgument("--disable-extensions");
            
            if (TestConfiguration.BrowserOptions.Headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }

            return new ChromeDriver(options);
        }
    }
}
