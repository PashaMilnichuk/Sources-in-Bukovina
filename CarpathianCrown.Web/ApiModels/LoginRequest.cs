using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Web.ApiModels;

public sealed class LoginRequest
{
    public string Identifier { get; set; } = "";

    public string Password { get; set; } = "";
}