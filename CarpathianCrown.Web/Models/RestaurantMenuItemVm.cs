namespace CarpathianCrown.Web.Models;

public sealed class RestaurantMenuItemVm
{
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string DescriptionUa { get; set; } = "";
    public string DescriptionEn { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = "";
}