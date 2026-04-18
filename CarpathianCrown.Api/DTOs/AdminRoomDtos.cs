using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.DTOs;

public record UpsertRoomRequest(
    string NameUa,
    string NameEn,
    string DescriptionUa,
    string DescriptionEn,
    decimal PricePerNight,
    int Capacity,
    string Status,
    string CoverImageUrl,
    string? Image2,
    string? Image3,
    string? Image4
);