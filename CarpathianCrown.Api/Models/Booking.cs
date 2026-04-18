using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models;

public class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending";

    public decimal RoomTotal { get; set; }
    public decimal ServicesTotal { get; set; }
    public decimal GrandTotal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<BookingService> BookingServices { get; set; } = new();
    public Review? Review { get; set; }
}