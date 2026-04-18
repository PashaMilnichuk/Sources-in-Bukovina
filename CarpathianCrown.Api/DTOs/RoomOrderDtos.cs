using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.DTOs;

public record CreateRoomOrderRequest(
    [Required] int BookingId,
    [Required] List<CreateRoomOrderLine> Lines
);

public record CreateRoomOrderLine(
    [Required] int ServiceItemId,
    [Required] int Quantity
);

public record ChangeRoomOrderStatusRequest(
    [Required] string Status
);