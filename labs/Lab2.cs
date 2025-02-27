using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

// no sleep, use implicit and excplicit waits

[TestFixture]
public class Lab2 
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        driver.Manage().Window.Maximize();

        // 1. Open https://demoqa.com/.
        driver.Navigate().GoToUrl("https://demoqa.com/");
    }

    // 2.1
    [Test]
    public void Test1()
    {
        // 3. Select the "Widgets" tab.
        var categorieContainer = driver.FindElement(By.XPath("//div[@class='category-cards']"));
        var categories = categorieContainer.FindElements(By.XPath("./*"));
        foreach(IWebElement category in categories)
        {
            if(category.FindElement(By.XPath(".//h5")).Text == "Widgets")
            {
                category.Click();
                break;
            } 
        }

        // 4. Choose the "Progress Bar" menu item.
        driver.FindElement(By.XPath("//span[text()='Progress Bar']")).Click();

        // 5. Click the "Start" button.
        driver.FindElement(By.XPath("//button[@id='startStopButton']")).Click();

        // 6. Wait until it reaches 100% and then click "Reset."
        IWebElement progressBar = driver.FindElement(By.XPath("//div[@role='progressbar']"));
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(driverInstance => progressBar.Text == "100%");
        driver.FindElement(By.XPath("//button[@id='resetButton']")).Click();
        Assert.That(progressBar.Text, Is.EqualTo("0%"));
    }

    // 2.2
    [Test]
    public void Test2()
    {
        // 3. Select the "Elements" tab.
        var categorieContainer = driver.FindElement(By.XPath("//div[@class='category-cards']"));
        var categories = categorieContainer.FindElements(By.XPath("./*"));
        foreach(IWebElement category in categories)
        {
            if(category.FindElement(By.XPath(".//h5")).Text == "Elements")
            {
                category.Click();
                break;
            } 
        }

        // 4. Choose the "Web Tables" menu item.
        driver.FindElement(By.XPath("//span[text()='Web Tables']")).Click();

        // 5. Add enough elements to create a second page in the pagination.
        IWebElement pageCountElement = driver.FindElement(By.XPath("//span[@class='-totalPages']"));
        var initalPageCount = pageCountElement.Text;
        IWebElement app = driver.FindElement(By.XPath("//div[@id='app']"));
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
        while(pageCountElement.Text == initalPageCount){
            driver.FindElement(By.XPath("//button[@id='addNewRecordButton']")).Click();
            wait.Until(driverInstance => app.GetAttribute("aria-hidden") != null); // make sure input fields have become visible
            driver.FindElement(By.XPath("//input[@id='firstName']")).SendKeys("Test first name");
            driver.FindElement(By.XPath("//input[@id='lastName']")).SendKeys("Test last name");
            driver.FindElement(By.XPath("//input[@id='userEmail']")).SendKeys("Test@email.com");
            driver.FindElement(By.XPath("//input[@id='age']")).SendKeys("99");
            driver.FindElement(By.XPath("//input[@id='salary']")).SendKeys("1234");
            driver.FindElement(By.XPath("//input[@id='department']")).SendKeys("Test department");
            driver.FindElement(By.XPath("//button[@id='submit']")).Click();
        }
        
        // 6. Navigate to the second page by clicking "Next."
        driver.FindElement(By.XPath("//div[@class='-next']")).FindElement(By.XPath(".//button")).Click();

        // 7. Delete an element on the second page.
        driver.FindElement(By.XPath("//span[@title='Delete']")).Click();

        // 8. Ensure that pagination automatically returns to the first page and that the number of pages is reduced to one.
        var pageCountAfterTaskCompletion = driver.FindElement(By.XPath("//input[@aria-label='jump to page']")).GetAttribute("value");
        Assert.That(pageCountAfterTaskCompletion, Is.EqualTo("1"));
    }
}