namespace CarpathianCrown.Web.Models;

public sealed class AdminRoomFormVm
{
    public int Id { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string DescriptionUa { get; set; } = "";
    public string DescriptionEn { get; set; } = "";
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = "Available";
    public string CoverImageUrl { get; set; } = "";
    public string? Image2 { get; set; }
    public string? Image3 { get; set; }
    public string? Image4 { get; set; }
}