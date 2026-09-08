using System.Text.Json;

namespace RestfulBooker.Tests.Config;

public sealed record TestSettings(string BaseUrl, string Browser, bool Headless)
{
    public static TestSettings Load()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "appsettings.json");

        var settings = JsonSerializer.Deserialize<TestSettings>(
            File.ReadAllText(path),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })
            ?? throw new InvalidOperationException(
                "Invalid appsettings.json");

        var baseUrl =
            Environment.GetEnvironmentVariable("BOOKER_BASE_URL")
            ?? settings.BaseUrl;

        var browser =
            Environment.GetEnvironmentVariable("BOOKER_BROWSER")
            ?? settings.Browser;

        var headlessValue =
            Environment.GetEnvironmentVariable("BOOKER_HEADLESS");

        var headless =
            bool.TryParse(headlessValue, out var parsedHeadless)
                ? parsedHeadless
                : settings.Headless;

        return settings with
        {
            BaseUrl = baseUrl,
            Browser = browser,
            Headless = headless
        };
    }
}