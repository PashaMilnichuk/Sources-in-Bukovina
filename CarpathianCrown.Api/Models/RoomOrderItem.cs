using System.ComponentModel.DataAnnotations.Schema;

namespace CarpathianCrown.Api.Models;

public class RoomOrderItem
{
    public int Id { get; set; }

    public int RoomOrderId { get; set; }
    public RoomOrder RoomOrder { get; set; } = null!;

    public int ServiceItemId { get; set; }
    public ServiceItem ServiceItem { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal LineTotal { get; set; }
}