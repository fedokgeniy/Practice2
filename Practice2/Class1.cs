using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace SeleniumTests
{
    [TestFixture]
    public class EHUSiteTests
    {
        private IWebDriver driver;
        private string baseUrl = "https://en.ehu.lt/";

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyNavigationToAboutEHU()
        {
            // Step 1
            driver.Navigate().GoToUrl(baseUrl);

            // Step 2
            IWebElement aboutLink = driver.FindElement(By.LinkText("About"));
            aboutLink.Click();

            // Checks
            Assert.That(driver.Url, Is.EqualTo("https://en.ehu.lt/about/"), "URL does not match expected.");

            Assert.That(driver.Title, Is.EqualTo("About"), "Page title does not match expected.");

            IWebElement heading = driver.FindElement(By.TagName("h1"));
            Assert.That(heading.Text, Is.EqualTo("About"), "Heading text does not match expected.");
        }

        [Test]
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

            Assert.That(driver.Url, Does.Contain("/?s=study+programs"), "URL does not contain expected search query.");

            IWebElement searchResults = driver.FindElement(By.ClassName("search-filter__result-count"));
            string classValue = searchResults.GetAttribute("class");
            string unexpectedText = "0 results found.";
            string actualText = searchResults.Text;

            Assert.That(unexpectedText, Is.Not.EqualTo(actualText), $"Expected: '{unexpectedText}', but was: '{actualText}'.");
        }

        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            driver.Navigate().GoToUrl(baseUrl);

            IWebElement switchIcon = driver.FindElement(By.ClassName("language-switcher"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(switchIcon).Perform();
            System.Threading.Thread.Sleep(1000);

            IWebElement langLink = driver.FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']"));
            langLink.Click();

            Assert.That(driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "URL does not match expected.");

            IWebElement heading = driver.FindElement(By.TagName("h1"));
            Assert.That(heading.Text, Is.EqualTo("Naujienos\r\nPaskutinės EHU naujienos"), "Heading text does not match expected.");
        }

        [Test]
        public void VerifyContactFormSubmission()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

            IWebElement emailElement = driver.FindElement(By.XPath("//a[contains(text(), 'franciskscarynacr@gmail.com')]"));
            Assert.That(emailElement, Is.Not.Null, "E-mail is not found.");

            IWebElement listItem = driver.FindElement(By.XPath("//strong[contains(text(), 'LT):')]/parent::li"));
            string phoneNumber = listItem.Text.Trim();
            Assert.That(phoneNumber, Is.EqualTo("Phone (LT): +370 68 771365"), "Lithuanian phone number is not found.");

            IWebElement phoneBYElem = driver.FindElement(By.XPath("//strong[contains(text(), 'Phone (BY')]/parent::li"));
            string phoneBY = phoneBYElem.Text.Trim();
            Assert.That(phoneBY, Is.EqualTo("Phone (BY): +375 29 5781488"), "Belarusian phone number is not found.");

            string[] socialNetworks = { "Facebook", "Telegram", "VK" };
            foreach (var network in socialNetworks)
            {
                IWebElement socialLink = driver.FindElement(By.XPath($"//a[contains(text(), '{network}')]"));
                Assert.That(socialLink, Is.Not.Null, $"Link to {network} is not found.");
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}


