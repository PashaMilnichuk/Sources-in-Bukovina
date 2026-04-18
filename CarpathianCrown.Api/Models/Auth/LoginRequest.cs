using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models.Auth;

public sealed class LoginRequest
{
    public string Identifier { get; set; } = "";
    public string Password { get; set; } = "";
}