using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class LangController : Controller
{
    [HttpPost]
    public IActionResult Set(string lang, string returnUrl = "/")
    {
        HttpContext.SetLang(lang);
        return LocalRedirect(returnUrl);
    }
}