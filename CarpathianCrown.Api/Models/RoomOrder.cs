using System.ComponentModel.DataAnnotations.Schema;

namespace CarpathianCrown.Api.Models;

public class RoomOrder
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Status { get; set; } = "New";

    [Column(TypeName = "numeric(12,2)")]
    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<RoomOrderItem> Items { get; set; } = new();
}