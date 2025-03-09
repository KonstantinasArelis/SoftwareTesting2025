using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace Lab3
{
    [TestFixture]
    public class Lab3
    {
        private IWebDriver driver;
        private readonly string userEmail = UserSetupFixture.userEmail;
        private readonly string password = UserSetupFixture.password;

        private static IEnumerable<string[]> TestDataProvider()
        {
            string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(BaseDirectory).ToString()).ToString()).ToString()).ToString() + "/Lab3/data/", "*.txt");
            foreach(string file in files)
            {
                yield return File.ReadAllLines(file);
            }
        }

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        }

        [Test]
        [TestCaseSource(nameof(TestDataProvider))]
        public void PurchaseTest(string[] data)
        {
            //  2.	Click "Log in"
            driver.FindElement(By.XPath("//a[@href='/login']")).Click();

            // 3.	Fill in the "Email" and "Password" fields and click "Log in"
            // user email with already existing address - 18bc074b-3df9-47da-9ea5-5d95eb0104b5@test.com
            driver.FindElement(By.Id("Email")).SendKeys(userEmail);
            driver.FindElement(By.Id("Password")).SendKeys(password);
            driver.FindElement(By.ClassName("login-button")).Click();

            // 4.	In the sidebar menu, select "Digital downloads"
            driver.FindElement(By.XPath("//a[@href='/digital-downloads']")).Click();

            // 5.	Add products to the cart by reading from a text file:
            IList<IWebElement> productItems = driver.FindElements(By.ClassName("product-item"));
            IWebElement blockingIcon = driver.FindElement(By.ClassName("ajax-loading-block-window"));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            foreach(string listItem in data)
            {
                foreach(IWebElement productItem in productItems)
                {
                    if(productItem.FindElement(By.XPath(".//h2[@class='product-title']/a")).Text == listItem)
                    {
                        productItem.FindElement(By.ClassName("product-box-add-to-cart-button")).Click();
                        break;
                    }
                }
                wait.Until(driverInstance => !blockingIcon.Displayed); // wait until the loading icon dissapears
            }

            //  6.	Open "Shopping cart"
            driver.FindElement(By.XPath("//a[@href='/cart']")).Click();

            // 7.	Check "I agree" and click "Checkout"
            driver.FindElement(By.Id("termsofservice")).Click();
            driver.FindElement(By.Id("checkout")).Click();

            // 8.	In "Billing Address", select an existing address or fill in a new one, then click "Continue"        
            IWebElement existingAdressSelectElement;
            try {
                existingAdressSelectElement = driver.FindElement(By.ClassName("address-select"));
                SelectElement countrySelect = new SelectElement(existingAdressSelectElement);
                countrySelect.SelectByIndex(0);
                driver.FindElement(By.ClassName("new-address-next-step-button")).Click();
            } catch(Exception ex){
                IWebElement countrySelectElement = driver.FindElement(By.Id("BillingNewAddress_CountryId"));
                SelectElement countrySelect = new SelectElement(countrySelectElement);
                countrySelect.SelectByText("United States");
                driver.FindElement(By.Id("BillingNewAddress_City")).SendKeys("TestCity");
                driver.FindElement(By.Id("BillingNewAddress_Address1")).SendKeys("TestAddress");
                driver.FindElement(By.Id("BillingNewAddress_ZipPostalCode")).SendKeys("12345");
                driver.FindElement(By.Id("BillingNewAddress_PhoneNumber")).SendKeys("123456789");
                driver.FindElement(By.ClassName("new-address-next-step-button")).Click();
            }
            
            //  9.	In "Payment Method", click "Continue"
            driver.FindElement(By.ClassName("payment-method-next-step-button")).Click();
            //  10.	In "Payment Information", click "Continue"
            driver.FindElement(By.ClassName("payment-info-next-step-button")).Click();

            //  11.	In "Confirm Order", click "Confirm"
            driver.FindElement(By.ClassName("confirm-order-next-step-button")).Click();

            //  12.	Ensure the order is successfully placed.
            var orderCompletionMessage = driver.FindElement(By.XPath("//div[@class='section order-completed']/div/strong")).Text;
            Assert.That(orderCompletionMessage, Is.EqualTo("Your order has been successfully processed!"));
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