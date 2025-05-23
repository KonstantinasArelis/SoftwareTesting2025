using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace Lab4
{
    public class Lab4()
    {
        private IWebDriver driver;
        private string testInput = "to know everything about computing and the internet";
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        } 

        [Test]
        public void productDescriptionSearch()
        {
            // open search
            driver.FindElement(By.XPath("//a[@href='/search']")).Click();
            
            // enter input information
            driver.FindElement(By.Id("As")).Click();

            driver.FindElement(By.Id("Sid")).Click();
            
            // enter test input for search
            driver.FindElement(By.Id("Q")).SendKeys(testInput);
            
            // start search
            driver.FindElement(By.ClassName("search-button")).Click();

            // pick first search result
            var results = driver.FindElement(By.ClassName("search-results"));
            IWebElement item = results.FindElement(By.ClassName("item-box"));

            // verify that result product contains the test input
            item.FindElement(By.XPath(".//h2[@class='product-title']/a")).Click();

            var description = driver.FindElement(By.XPath("//div[@class='full-description']/p")).Text;

            Assert.That(description.Contains(testInput));
        }

        [TearDown]
        public void TearDown()
        {
            if(driver != null)
            {
                driver.Quit();
            }
        }
    }
}