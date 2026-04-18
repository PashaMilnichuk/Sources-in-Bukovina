using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models;

public class Room
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string NameUa { get; set; } = "";

    [Required, MaxLength(120)]
    public string NameEn { get; set; } = "";

    [Required]
    public string DescriptionUa { get; set; } = "";

    [Required]
    public string DescriptionEn { get; set; } = "";

    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Available";

    public string CoverImageUrl { get; set; } = "";

    public List<Booking> Bookings { get; set; } = new();
    public string? Image2 { get; set; }
    public string? Image3 { get; set; }
    public string? Image4 { get; set; }
}