namespace CarpathianCrown.Web.Models;

public sealed class AdminServiceFormVm
{
    public int Id { get; set; }
    public string Category { get; set; } = "";
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string DescriptionUa { get; set; } = "";
    public string DescriptionEn { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
}