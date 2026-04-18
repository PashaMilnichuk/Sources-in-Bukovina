using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Web.ApiModels;

public sealed class RegisterRequest
{
    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = "";

    public string? Login { get; set; }

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = "";

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public bool AcceptPersonalData { get; set; }
}