using RestSharp;
using RestfulBooker.Tests.Config;
using RestfulBooker.Tests.Models;
using RestfulBooker.Tests.Support;
using System.Text.Json;

namespace RestfulBooker.Tests.Api;

public sealed class BookerApiClient
{
    private readonly RestClient _client =
        new(TestSettings.Load().BaseUrl);

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private async Task<RestResponse> Execute(RestRequest request)
    {
        Log.Instance.Information(
            "{Method} {Resource}",
            request.Method,
            request.Resource);

        var response = await _client.ExecuteAsync(request);

        Log.Instance.Information(
            "Status {Status}; Body {Body}",
            (int)response.StatusCode,
            response.Content);

        return response;
    }

    private T Deserialize<T>(RestResponse response)
    {
        if (string.IsNullOrWhiteSpace(response.Content))
        {
            throw new InvalidOperationException(
                "The API returned an empty response.");
        }

        return JsonSerializer.Deserialize<T>(
            response.Content,
            _jsonOptions)
            ?? throw new InvalidOperationException(
                $"Could not deserialize response to {typeof(T).Name}.");
    }

    public Task<RestResponse> Ping()
    {
        return Execute(
            new RestRequest("/ping", Method.Get));
    }

    public async Task<AuthResponse> Authenticate()
    {
        var request = new RestRequest(
            "/auth",
            Method.Post)
            .AddJsonBody(
                new AuthRequest(
                    "admin",
                    "password123"));

        var response = await Execute(request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return Deserialize<AuthResponse>(response);
    }

    public async Task<CreateBookingResponse> Create(
        Booking booking)
    {
        var request = new RestRequest(
            "/booking",
            Method.Post)
            .AddJsonBody(booking);

        var response = await Execute(request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return Deserialize<CreateBookingResponse>(response);
    }

    public Task<RestResponse> Get(int id)
    {
        return Execute(
            new RestRequest(
                $"/booking/{id}",
                Method.Get));
    }

    public async Task<Booking> GetBooking(int id)
    {
        var response = await Get(id);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return Deserialize<Booking>(response);
    }

    private RestRequest AuthRequest(
        string path,
        Method method,
        string token)
    {
        return new RestRequest(path, method)
            .AddCookie("token", token);
    }

    public async Task<Booking> Replace(
        int id,
        string token,
        Booking booking)
    {
        var request = AuthRequest(
                $"/booking/{id}",
                Method.Put,
                token)
            .AddJsonBody(booking);

        var response = await Execute(request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return Deserialize<Booking>(response);
    }

    public async Task<Booking> Patch(
        int id,
        string token,
        object body)
    {
        var request = AuthRequest(
                $"/booking/{id}",
                Method.Patch,
                token)
            .AddJsonBody(body);

        var response = await Execute(request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return Deserialize<Booking>(response);
    }

    public Task<RestResponse> Delete(
        int id,
        string token)
    {
        return Execute(
            AuthRequest(
                $"/booking/{id}",
                Method.Delete,
                token));
    }

    public Task<RestResponse> ReplaceWithoutToken(
        int id,
        Booking booking)
    {
        return Execute(
            new RestRequest(
                $"/booking/{id}",
                Method.Put)
                .AddJsonBody(booking));
    }
}