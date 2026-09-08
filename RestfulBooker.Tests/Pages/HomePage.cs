using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RestfulBooker.Tests.Pages;

public sealed class HomePage(IWebDriver driver)
{
    private readonly WebDriverWait _wait =
        new(driver, TimeSpan.FromSeconds(10));

    public HomePage Open(string url)
    {
        driver.Navigate().GoToUrl(url);

        _wait.Until(d =>
            !string.IsNullOrWhiteSpace(d.Title));

        return this;
    }

    public string Title => driver.Title;

    public string BodyText =>
        _wait.Until(d =>
            d.FindElement(By.TagName("body"))).Text;

    public bool HasHeading =>
        driver.FindElements(
            By.CssSelector("h1,h2")).Count > 0;
}