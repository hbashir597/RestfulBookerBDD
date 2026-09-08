using RestSharp;
using RestfulBooker.Tests.Models;

namespace RestfulBooker.Tests.Support;

public sealed class ScenarioState
{
    public string Token { get; set; } = "";

    public int BookingId { get; set; }

    public Booking? Expected { get; set; }

    public Booking? Latest { get; set; }

    public RestResponse? LastResponse { get; set; }
}