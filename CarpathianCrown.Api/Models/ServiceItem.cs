using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models;

public class ServiceItem
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string NameUa { get; set; } = "";

    [Required, MaxLength(120)]
    public string NameEn { get; set; } = "";

    public string DescriptionUa { get; set; } = "";
    public string DescriptionEn { get; set; } = "";

    public decimal Price { get; set; }

    [Required, MaxLength(40)]
    public string Category { get; set; } = "Other";

    public bool IsActive { get; set; } = true;

    public List<BookingService> BookingServices { get; set; } = new();

    public string? ImageUrl { get; set; }
}