using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.DTOs;

public record RegisterRequest(
    [Required, EmailAddress] string Email,
    string? Login,
    [Required, MinLength(6)] string Password,
    string FirstName,
    string LastName,
    bool AcceptPersonalData
);

public record LoginRequest(
    string Identifier, 
    string Password
);

public record AuthResponse(
    string Token,
    string Role,
    int UserId
);