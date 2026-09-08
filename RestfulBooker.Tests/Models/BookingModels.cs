namespace RestfulBooker.Tests.Models;

public sealed record BookingDates(
    string checkin,
    string checkout
);

public sealed record Booking(
    string firstname,
    string lastname,
    int totalprice,
    bool depositpaid,
    BookingDates bookingdates,
    string additionalneeds
);

public sealed record CreateBookingResponse(
    int bookingid,
    Booking booking
);

public sealed record AuthRequest(
    string username,
    string password
);

public sealed record AuthResponse(
    string? token,
    string? reason
);