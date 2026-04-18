using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Web.Models;

public class RegisterViewModel
{
    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = "";

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = "";

    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    public bool AcceptPersonalData { get; set; }
}