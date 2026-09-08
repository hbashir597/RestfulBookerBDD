using AventStack.ExtentReports;
using Reqnroll;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.Hooks;

[Binding]
public sealed class ExtentHooks(ScenarioContext context)
{
    private ExtentTest? _test;

    private static readonly object ReportLock = new();

    [BeforeScenario(Order = -50)]
    public void Before()
    {
        lock (ReportLock)
        {
            _test = ExtentReport.Instance
                .CreateTest(context.ScenarioInfo.Title);

            foreach (var tag in context.ScenarioInfo.CombinedTags)
            {
                _test.AssignCategory(tag);
            }
        }
    }

    [AfterStep]
    public void Step()
    {
        lock (ReportLock)
        {
            _test?.Info(
                context.StepContext.StepInfo.Text);
        }
    }

    [AfterScenario(Order = 200)]
    public void After()
    {
        lock (ReportLock)
        {
            if (context.TestError is null)
            {
                _test?.Pass("Scenario passed");
            }
            else
            {
                _test?.Fail(context.TestError);
            }

            ExtentReport.Instance.Flush();
        }
    }
}