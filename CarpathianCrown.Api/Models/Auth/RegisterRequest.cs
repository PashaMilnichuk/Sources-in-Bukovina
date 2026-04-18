using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models.Auth;

public sealed class RegisterRequest
{
    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = "";

    [MaxLength(40)]
    public string? Login { get; set; }

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = "";

    [MaxLength(60)]
    public string? FirstName { get; set; }

    [MaxLength(60)]
    public string? LastName { get; set; }

    [MaxLength(30)]
    public string Phone { get; set; }

    [Required]
    public bool AcceptPersonalData { get; set; }
}