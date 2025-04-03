using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace SeleniumTests
{
    [TestFixture]
    public class AboutEHUTest
    {
        private IWebDriver driver;

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
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            // Step 2
            IWebElement aboutLink = driver.FindElement(By.LinkText("About EHU"));
            aboutLink.Click();

            // Checks
            Assert.That(driver.Url, Is.EqualTo("https://en.ehu.lt/about/"), "URL does not match expected.");

            Assert.That(driver.Title, Is.EqualTo("About EHU"), "Page title does not match expected.");

            IWebElement header = driver.FindElement(By.TagName("h1"));
            Assert.That(header.Text, Is.EqualTo("About European Humanities University"), "Header text does not match expected.");
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}


