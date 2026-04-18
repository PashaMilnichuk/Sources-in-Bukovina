using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.DTOs;

public record UpsertContentPageRequest(
    [Required] string Slug,
    [Required] string TitleUa,
    [Required] string TitleEn,
    [Required] string HtmlUa,
    [Required] string HtmlEn,
    string? HeroImageUrl
);

public record UpsertServiceItemRequest(
    int? Id,
    [Required] string NameUa,
    [Required] string NameEn,
    string? DescriptionUa,
    string? DescriptionEn,
    [Required] string Category,
    decimal Price,
    bool IsActive,
    string? ImageUrl
);