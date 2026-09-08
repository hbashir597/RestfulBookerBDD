using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestfulBooker.Tests.Config;

namespace RestfulBooker.Tests.Support;

public sealed class DriverFactory : IDisposable
{
    public IWebDriver? Driver { get; private set; }

    public IWebDriver Create()
    {
        var settings = TestSettings.Load();

        var options = new ChromeOptions();

        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArguments(
            "--window-size=1440,1000",
            "--disable-notifications");

        Driver = new ChromeDriver(options);

        return Driver;
    }

    public byte[] Screenshot()
    {
        return ((ITakesScreenshot)Driver!)
            .GetScreenshot()
            .AsByteArray;
    }

    public void Dispose()
    {
        try
        {
            Driver?.Quit();
        }
        finally
        {
            Driver?.Dispose();
            Driver = null;
        }
    }
}