using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.NetworkInformation;

namespace Lab3
{
    [SetUpFixture]
    public class UserSetupFixture
    {
        public static readonly string userEmail = Guid.NewGuid().ToString() + "@test.com";
        public static readonly string firstName = "TestName";
        public static readonly string lastName = "LastName";
        public static readonly string password = "TestPassword";
        private IWebDriver userCreationDriver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            userCreationDriver = new ChromeDriver();
            userCreationDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            userCreationDriver.Manage().Window.Maximize();
            userCreationDriver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

            // 2.	Click "Log in"
            userCreationDriver.FindElement(By.XPath("//a[@href='/login']")).Click();
            
            // 3.	Click "Register" in the "New Customer" section
            userCreationDriver.FindElement(By.ClassName("register-block")).FindElement(By.XPath("//input[@value='Register']")).Click();

            // 4.	Fill in the registration form fields
            IWebElement registrationForm = userCreationDriver.FindElement(By.ClassName("registration-page"));
            registrationForm.FindElement(By.Id("gender-male")).Click();
            registrationForm.FindElement(By.Id("FirstName")).SendKeys(firstName);
            registrationForm.FindElement(By.Id("LastName")).SendKeys(lastName);
            registrationForm.FindElement(By.Id("Email")).SendKeys(userEmail);
            registrationForm.FindElement(By.Id("Password")).SendKeys(password);
            registrationForm.FindElement(By.Id("ConfirmPassword")).SendKeys(password);
            registrationForm.FindElement(By.Id("register-button")).Click();

            userCreationDriver.FindElement(By.ClassName("register-continue-button")).Click();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if(userCreationDriver != null)
            {
                userCreationDriver.Quit();
            }
        }
    }
}