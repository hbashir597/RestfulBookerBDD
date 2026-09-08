using System.Net;
using Reqnroll;
using RestfulBooker.Tests.Api;
using RestfulBooker.Tests.Models;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.StepDefinitions;

[Binding]
public sealed class ApiSteps(BookerApiClient api, ScenarioState state)
{
    private Booking Unique() =>
        new(
            "Auto" + Guid.NewGuid().ToString("N")[..8],
            "Tester",
            450,
            true,
            new BookingDates(
                DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd"),
                DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd")),
            "Breakfast");

    [Given("the booking service is available")]
    public async Task Available()
    {
        Assert.That(
            (int)(await api.Ping()).StatusCode,
            Is.EqualTo(201));
    }

    [When("I call the booking service health endpoint")]
    public async Task Ping()
    {
        state.LastResponse = await api.Ping();
    }

    [Then("the health response should be successful")]
    public void Health()
    {
        Assert.That(
            (int)state.LastResponse!.StatusCode,
            Is.EqualTo(201));
    }

    [Given("I have a valid admin token")]
    public async Task Token()
    {
        state.Token = (await api.Authenticate()).token!;

        Assert.That(
            state.Token,
            Is.Not.Empty);
    }

    [When("I create a unique booking")]
    [Given("I created a unique booking")]
    public async Task Create()
    {
        state.Expected = Unique();

        var response = await api.Create(state.Expected);

        state.BookingId = response.bookingid;
        state.Latest = response.booking;
    }

    [Then("the booking should be created")]
    public void Created()
    {
        Assert.Multiple(() =>
        {
            Assert.That(
                state.BookingId,
                Is.GreaterThan(0));

            Assert.That(
                state.Latest!.firstname,
                Is.EqualTo(state.Expected!.firstname));
        });
    }

    [Then("the booking should be retrievable")]
    public async Task Retrieve()
    {
        state.Latest =
            await api.GetBooking(state.BookingId);

        Assert.That(
            state.Latest.lastname,
            Is.EqualTo("Tester"));
    }

    [When("I replace the booking details")]
    public async Task Replace()
    {
        state.Expected = state.Expected! with
        {
            totalprice = 600,
            additionalneeds = "Breakfast and late checkout",
            bookingdates = new BookingDates(
                DateTime.UtcNow.AddDays(8).ToString("yyyy-MM-dd"),
                DateTime.UtcNow.AddDays(12).ToString("yyyy-MM-dd"))
        };

        await api.Replace(
            state.BookingId,
            state.Token,
            state.Expected);
    }

    [Then("the replaced details should be persisted")]
    public async Task VerifyReplace()
    {
        state.Latest =
            await api.GetBooking(state.BookingId);

        Assert.Multiple(() =>
        {
            Assert.That(
                state.Latest.totalprice,
                Is.EqualTo(600));

            Assert.That(
                state.Latest.additionalneeds,
                Is.EqualTo("Breakfast and late checkout"));
        });
    }

    [When("I partially update the price and additional needs")]
    public async Task Patch()
    {
        await api.Patch(
            state.BookingId,
            state.Token,
            new
            {
                totalprice = 700,
                additionalneeds = "Airport pickup"
            });
    }

    [Then("only the selected booking fields should change")]
    public async Task VerifyPatch()
    {
        state.Latest =
            await api.GetBooking(state.BookingId);

        Assert.Multiple(() =>
        {
            Assert.That(
                state.Latest.totalprice,
                Is.EqualTo(700));

            Assert.That(
                state.Latest.additionalneeds,
                Is.EqualTo("Airport pickup"));

            Assert.That(
                state.Latest.firstname,
                Is.EqualTo(state.Expected!.firstname));
        });
    }

    [When("I delete the booking")]
    public async Task Delete()
    {
        state.LastResponse =
            await api.Delete(
                state.BookingId,
                state.Token);

        Assert.That(
            (int)state.LastResponse.StatusCode,
            Is.EqualTo(201));
    }

    [Then("retrieving the deleted booking should return 404")]
    public async Task Deleted()
    {
        var response =
            await api.Get(state.BookingId);

        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.NotFound));
    }

    [When("I try to replace it without authentication")]
    public async Task NoAuth()
    {
        state.LastResponse =
            await api.ReplaceWithoutToken(
                state.BookingId,
                Unique());
    }

    [Then("the update response should be 401 or 403")]
    public void Rejected()
    {
        Assert.That(
            (int)state.LastResponse!.StatusCode,
            Is.AnyOf(401, 403));
    }
}