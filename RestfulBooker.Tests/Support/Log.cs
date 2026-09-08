using Serilog;

namespace RestfulBooker.Tests.Support;

public static class Log
{
    public static ILogger Instance { get; } =
        new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/automation-.log",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();
}