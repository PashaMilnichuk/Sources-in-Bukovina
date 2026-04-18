using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.DTOs;

public record CreateReviewRequest(int BookingId, int Rating, string Comment);