using Reqnroll;
using RestfulBooker.Tests.Config;
using RestfulBooker.Tests.Pages;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.StepDefinitions;

[Binding]
public sealed class UiSteps(DriverFactory drivers)
{
    private HomePage? _page;

    [Given("I open the RESTful Booker landing page")]
    public void Open()
    {
        _page = new HomePage(
            drivers.Create())
            .Open(TestSettings.Load().BaseUrl);
    }

    [Then("the page should identify RESTful Booker")]
    public void Identify()
    {
        Assert.That(
            _page!.Title.Contains(
                "restful",
                StringComparison.OrdinalIgnoreCase)
            ||
            _page.BodyText.Contains(
                "restful-booker",
                StringComparison.OrdinalIgnoreCase));
    }

    [Then("the page should contain API learning content")]
    public void Content()
    {
        Assert.That(
            _page!.HasHeading,
            Is.True);

        Assert.That(
            _page.BodyText,
            Does.Contain("API").IgnoreCase);
    }
}