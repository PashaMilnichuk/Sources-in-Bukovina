using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Web.Models;

public class LoginViewModel
{
    public string Identifier { get; set; } = "";

    public string Password { get; set; } = "";
}