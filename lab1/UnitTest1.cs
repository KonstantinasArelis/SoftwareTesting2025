namespace lab1;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

public class Tests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
    }

    [Test]
    public void Test1()
    {
        IWebElement giftcardLink = driver.FindElement(By.XPath("//a[@href='/gift-cards']"));
        giftcardLink.Click();
        IList<IWebElement> giftcardProductList = driver.FindElements(By.XPath("//div[@class='product-item']"));
        foreach(IWebElement giftcardProduct in giftcardProductList){
            var price = giftcardProduct.FindElement(By.XPath(".//span[@class='price actual-price']")).Text;
            if(Convert.ToDouble(price) > 99)
            {
                giftcardProduct.FindElement(By.XPath(".//input[@value='Add to cart']")).Click();
                break;
            }
        }

        driver.FindElement(By.XPath("//input[@class='recipient-name']")).SendKeys("name of recipient");
        driver.FindElement(By.XPath("//input[@class='sender-name']")).SendKeys("name of sender");
        driver.FindElement(By.XPath("//input[@class='qty-input']")).Clear();
        driver.FindElement(By.XPath("//input[@class='qty-input']")).SendKeys("5000");
        

        driver.FindElement(By.XPath("//input[@value='Add to cart']")).Click();
        Thread.Sleep(500); // why does it not work without sleep
        driver.FindElement(By.XPath("//input[@value='Add to wishlist']")).Click();

        driver.FindElement(By.XPath("//a[@href='/jewelry']")).Click();
        driver.FindElement(By.XPath("//a[@href='/create-it-yourself-jewelry']")).Click();

        IWebElement attributes = driver.FindElement(By.XPath("//div[@class='overview']/div[@class='attributes']"));
        IWebElement materialSelector = attributes.FindElement(By.XPath(".//select"));
        SelectElement select = new SelectElement(materialSelector);
        select.SelectByText("Silver (1 mm)");
        attributes.FindElement(By.XPath(".//input[@class='textbox']")).SendKeys("80");

        IList<IWebElement> options = attributes.FindElements(By.XPath(".//ul[@class='option-list']/li"));
        foreach(IWebElement option in options)
        {
            if (option.FindElement(By.XPath(".//label")).Text == "Star")
            {
                option.FindElement(By.XPath(".//input")).Click();
                break;
            }
        }
        IWebElement quantity = driver.FindElement(By.XPath("//input[@class='qty-input']"));
        quantity.Clear();
        quantity.SendKeys("26");

        driver.FindElement(By.XPath("//input[@value='Add to cart']")).Click();
        Thread.Sleep(500); // why does it not work without sleep
        driver.FindElement(By.XPath("//input[@value='Add to wishlist']")).Click();

        driver.FindElement(By.XPath("//a[@href='/wishlist']")).Click();

        IList<IWebElement> itemsToBeAddedToCart = driver.FindElements(By.XPath("//tr[@class='cart-item-row']"));
        foreach(IWebElement item in itemsToBeAddedToCart)
        {
            item.FindElement(By.XPath(".//input[@name='addtocart']")).Click();
        }
        
        driver.FindElement(By.XPath("//input[@name='addtocartbutton']")).Click();


        IList<IWebElement> cartTotalDetailRows = driver.FindElements(By.XPath("//table[@class='cart-total']/tbody/tr"));
        bool subtotalIsCorrect = false;
        foreach(IWebElement totalDetailRow in cartTotalDetailRows)
        {
            string subtotalType = totalDetailRow.FindElement(By.XPath(".//span[@class='nobr']")).Text;
            if(subtotalType == "Sub-Total:")
            {
                string subtotal = totalDetailRow.FindElement(By.XPath(".//span[@class='product-price']")).Text;
                if(subtotal == "1002600.00"){
                    subtotalIsCorrect = true;
                } else {
                    Assert.Fail(); // subtotal is wrong
                }
            }
        }
        Assert.That(subtotalIsCorrect, Is.EqualTo(true)); // subtotal is actually present on page and is correct
    }
}