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

        //driver.Quit();
        Assert.Pass();
    }
}