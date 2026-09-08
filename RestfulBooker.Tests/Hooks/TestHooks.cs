using Allure.Net.Commons;
using Reqnroll;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.Hooks;

[Binding]
public sealed class TestHooks(
    DriverFactory drivers,
    ScenarioContext context,
    ScenarioState state)
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

    [AfterScenario("api", Order = 150)]
    public void AfterApi()
    {
        if (context.TestError is null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(state.LastRequestBody))
        {
            AllureApi.AddAttachment(
                "REST request body",
                "application/json",
                System.Text.Encoding.UTF8.GetBytes(
                    state.LastRequestBody),
                ".json");
        }

        if (!string.IsNullOrWhiteSpace(state.LastResponseBody))
        {
            AllureApi.AddAttachment(
                "REST response body",
                "application/json",
                System.Text.Encoding.UTF8.GetBytes(
                    state.LastResponseBody),
                ".json");
        }
    }
}