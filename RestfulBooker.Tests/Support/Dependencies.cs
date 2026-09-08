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
        var state = new ScenarioState();

        container.RegisterInstanceAs(state);
        container.RegisterInstanceAs(new BookerApiClient(state));
        container.RegisterInstanceAs(new DriverFactory());
    }
}