using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

[TestFixture]
public class Lab1
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
        // 1. Open the website https://demowebshop.tricentis.com/.
        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
    }

    [Test]
    public void Test1()
    {   

        // 2. Click on 'Gift Cards' in the left menu.
        IWebElement giftcardLink = driver.FindElement(By.XPath("//a[@href='/gift-cards']"));
        giftcardLink.Click();

        // 3. Select a product with a price higher than 99. The selection should not be hardcoded, as product prices may change in the future.
        IList<IWebElement> giftcardProductList = driver.FindElements(By.XPath("//div[@class='product-item']"));
        foreach(IWebElement giftcardProduct in giftcardProductList){
            var price = giftcardProduct.FindElement(By.XPath(".//span[@class='price actual-price']")).Text;
            if(Convert.ToDouble(price) > 99)
            {
                giftcardProduct.FindElement(By.XPath(".//input[@value='Add to cart']")).Click();
                break;
            }
        }
        // 4. Fill in the fields 'Recipient's Name:' and 'Your Name:' as desired.
        driver.FindElement(By.XPath("//input[@class='recipient-name']")).SendKeys("name of recipient");
        driver.FindElement(By.XPath("//input[@class='sender-name']")).SendKeys("name of sender");

        // 5. Enter '5000' in the 'Qty' text field.
        driver.FindElement(By.XPath("//input[@class='qty-input']")).Clear();
        driver.FindElement(By.XPath("//input[@class='qty-input']")).SendKeys("5000");
       
        // 6. Click the 'Add to cart' button.
        driver.FindElement(By.XPath("//input[@value='Add to cart']")).Click();

        //7. Click the 'Add to wish list' button.
        Thread.Sleep(500);
        driver.FindElement(By.XPath("//input[@value='Add to wishlist']")).Click();

        // 8. Click on 'Jewelry' in the left menu.
        Thread.Sleep(5000); // wait for header message to dissapear
        driver.FindElement(By.XPath("//a[@href='/jewelry']")).Click();

        // 9. Click the 'Create Your Own Jewelry' link.
        driver.FindElement(By.XPath("//a[@href='/create-it-yourself-jewelry']")).Click();

        // 10. Select the following values:'Material' - 'Silver 1mm', 'Length in cm' - '80', 'Pendant' - 'Star'
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

        // 11. Enter '26' in the 'Qty' text field.
        IWebElement quantity = driver.FindElement(By.XPath("//input[@class='qty-input']"));
        quantity.Clear();
        quantity.SendKeys("26");

        // 12. Click the 'Add to cart' button.
        driver.FindElement(By.XPath("//input[@value='Add to cart']")).Click();

        // 13. Click the 'Add to wish list' button.
        Thread.Sleep(5000); // wait for header message to dissapear
        driver.FindElement(By.XPath("//input[@value='Add to wishlist']")).Click();

        // 14. Click the 'Wishlist' link at the top of the page.
        driver.FindElement(By.XPath("//a[@href='/wishlist']")).Click();

        // 15. Check the 'Add to cart' checkboxes for both products.
        IList<IWebElement> itemsToBeAddedToCart = driver.FindElements(By.XPath("//tr[@class='cart-item-row']"));
        foreach(IWebElement item in itemsToBeAddedToCart)
        {
            item.FindElement(By.XPath(".//input[@name='addtocart']")).Click();
        }
        
        // 16. Click the 'Add to cart' button.
        driver.FindElement(By.XPath("//input[@name='addtocartbutton']")).Click();

        // 17. On the Shopping Cart page, verify that the 'Sub-Total' value is '1002600.00 
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
                    Assert.Fail("subtatal is wrong"); // subtotal is wrong
                }
            }
        }
        Assert.That(subtotalIsCorrect, Is.EqualTo(true)); // subtotal is actually present on page and is correct
    }
}