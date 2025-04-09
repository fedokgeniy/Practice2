using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using Xunit;

namespace SeleniumTests
{
    public class EHUSiteTests : IDisposable
    {
        private IWebDriver driver;
        private string baseUrl = "https://en.ehu.lt/";

        public EHUSiteTests()
        {
            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
        }

        [Fact]
        public void VerifyNavigationToAboutEHU()
        {
            driver.Navigate().GoToUrl(baseUrl);

            IWebElement aboutLink = driver.FindElement(By.LinkText("About"));
            aboutLink.Click();

            Assert.Equal("https://en.ehu.lt/about/", driver.Url);
            Assert.Equal("About", driver.Title);

            IWebElement heading = driver.FindElement(By.TagName("h1"));
            Assert.Equal("About", heading.Text);
        }

        [Fact]
        public void VerifySearchFunctionality()
        {
            driver.Navigate().GoToUrl(baseUrl);

            IWebElement searchIcon = driver.FindElement(By.ClassName("header-search"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(searchIcon).Perform();
            System.Threading.Thread.Sleep(1000);

            IWebElement searchBox = driver.FindElement(By.Name("s"));
            searchBox.SendKeys("study programs");
            searchBox.SendKeys(Keys.Enter);

            Assert.Contains("/?s=study+programs", driver.Url);

            IWebElement searchResults = driver.FindElement(By.ClassName("search-filter__result-count"));
            string unexpectedText = "0 results found.";
            string actualText = searchResults.Text;

            Assert.NotEqual(unexpectedText, actualText);
        }

        [Fact]
        public void VerifyLanguageChangeFunctionality()
        {
            driver.Navigate().GoToUrl(baseUrl);

            IWebElement switchIcon = driver.FindElement(By.ClassName("language-switcher"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(switchIcon).Perform();
            System.Threading.Thread.Sleep(1000);

            IWebElement langLink = driver.FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']"));
            langLink.Click();

            Assert.Equal("https://lt.ehu.lt/", driver.Url);

            IWebElement heading = driver.FindElement(By.TagName("h1"));
            Assert.Equal("Naujienos\r\nPaskutinės EHU naujienos", heading.Text);
        }

        [Fact]
        public void VerifyContactFormSubmission()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

            IWebElement emailElement = driver.FindElement(By.XPath("//a[contains(text(), 'franciskscarynacr@gmail.com')]"));
            Assert.NotNull(emailElement);

            IWebElement listItem = driver.FindElement(By.XPath("//strong[contains(text(), 'LT):')]/parent::li"));
            string phoneNumber = listItem.Text.Trim();
            Assert.Equal("Phone (LT): +370 68 771365", phoneNumber);

            IWebElement phoneBYElem = driver.FindElement(By.XPath("//strong[contains(text(), 'Phone (BY')]/parent::li"));
            string phoneBY = phoneBYElem.Text.Trim();
            Assert.Equal("Phone (BY): +375 29 5781488", phoneBY);

            string[] socialNetworks = { "Facebook", "Telegram", "VK" };
            foreach (var network in socialNetworks)
            {
                IWebElement socialLink = driver.FindElement(By.XPath($"//a[contains(text(), '{network}')]"));
                Assert.NotNull(socialLink);
            }
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}