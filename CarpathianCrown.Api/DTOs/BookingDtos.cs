using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.DTOs;

public record CreateBookingRequest(
    [Required] int RoomId,
    [Required] DateOnly CheckIn,
    [Required] DateOnly CheckOut
);

public record AddServicesRequest(
    [Required] int BookingId,
    List<ServiceLine> Lines
);

public record ServiceLine(
    int ServiceItemId,
    int Quantity
);

public record ChangeBookingStatusRequest(string Status);