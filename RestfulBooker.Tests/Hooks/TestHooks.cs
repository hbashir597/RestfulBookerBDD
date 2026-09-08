using Allure.Net.Commons;
using Reqnroll;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.Hooks;

[Binding]
public sealed class TestHooks(
    DriverFactory drivers,
    ScenarioContext context)
{
    [AfterScenario("ui", Order = 100)]
    public void AfterUi()
    {
        try
        {
            if (context.TestError is not null &&
                drivers.Driver is not null)
            {
                var bytes = drivers.Screenshot();

                context.ScenarioContainer
                    .Resolve<ScenarioContext>()
                    .Add("failureScreenshot", bytes);

                AllureApi.AddAttachment(
                    "Failure screenshot",
                    "image/png",
                    bytes,
                    ".png");
            }
        }
        finally
        {
            drivers.Dispose();
        }
    }
}