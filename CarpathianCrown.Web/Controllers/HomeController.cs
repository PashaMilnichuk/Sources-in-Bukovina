using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApiClient _api;
    public HomeController(ApiClient api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var lang = HttpContext.GetLang();
        var rooms = await _api.Get<List<RoomDto>>($"/api/rooms?lang={lang}");
        ViewBag.Auth = HttpContext.GetAuth();
        return View(rooms.Take(6).ToList());
    }
}