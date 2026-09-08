using System.Text.Json;

namespace RestfulBooker.Tests.Config;

public sealed record TestSettings(string BaseUrl, string Browser, bool Headless)
{
    public static TestSettings Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        return JsonSerializer.Deserialize<TestSettings>(
            File.ReadAllText(path),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )
        ?? throw new InvalidOperationException("Invalid appsettings.json");
    }
}