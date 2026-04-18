using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models;

public class ContentPage
{
    public int Id { get; set; }

    [Required, MaxLength(40)]
    public string Slug { get; set; } = "";

    [Required, MaxLength(120)]
    public string TitleUa { get; set; } = "";

    [Required, MaxLength(120)]
    public string TitleEn { get; set; } = "";

    [Required]
    public string HtmlUa { get; set; } = "";

    [Required]
    public string HtmlEn { get; set; } = "";

    public string HeroImageUrl { get; set; } = "";
}