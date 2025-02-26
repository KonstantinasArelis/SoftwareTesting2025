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
        driver.Navigate().GoToUrl("https://demoqa.com/");
    }

    // 2.1
    [Test]
    public void Test1()
    {
        Assert.Pass("Test1 complete");
    }

    // 2.2
    [Test]
    public void Test2()
    {
        Assert.Pass("Test2 complete");
    }
}