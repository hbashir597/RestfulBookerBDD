using Reqnroll;
using Reqnroll.BoDi;
using RestfulBooker.Tests.Api;

namespace RestfulBooker.Tests.Support;

[Binding]
public sealed class Dependencies
{
    [BeforeScenario(Order = -100)]
    public void Register(IObjectContainer container)
    {
        container.RegisterInstanceAs(new ScenarioState());
        container.RegisterInstanceAs(new BookerApiClient());
        container.RegisterInstanceAs(new DriverFactory());
    }
}