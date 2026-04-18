using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class PublicController : Controller
{
    private readonly ApiClient _api;

    public PublicController(ApiClient api)
    {
        _api = api;
    }

    private async Task<ContentPageDto> GetPage(string slug)
    {
        var lang = HttpContext.GetLang();
        return await _api.Get<ContentPageDto>($"/api/content/{slug}?lang={lang}");
    }

    [HttpGet]
    public async Task<IActionResult> About()
    {
        ViewBag.Auth = HttpContext.GetAuth();
        ViewBag.Slug = "about";
        return View("Page", await GetPage("about"));
    }

    [HttpGet]
    public async Task<IActionResult> Contacts()
    {
        ViewBag.Auth = HttpContext.GetAuth();
        ViewBag.Slug = "contacts";
        return View("Page", await GetPage("contacts"));
    }

    [HttpGet]
    public async Task<IActionResult> Restaurant()
    {
        var lang = HttpContext.GetLang();
        var auth = HttpContext.GetAuth();

        ViewBag.Auth = auth;
        ViewBag.Page = await GetPage("restaurant");

        List<ServiceItemDto> items;

        try
        {
            items = await _api.Get<List<ServiceItemDto>>(
                $"/api/services/restaurant?lang={lang}",
                null);
        }
        catch
        {
            items = new List<ServiceItemDto>();
        }

        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Services()
    {
        var lang = HttpContext.GetLang();
        var auth = HttpContext.GetAuth();

        ViewBag.Auth = auth;
        ViewBag.Page = await GetPage("services");

        List<ServiceItemDto> services;

        try
        {
            services = await _api.Get<List<ServiceItemDto>>(
                $"/api/services?lang={lang}",
                auth.IsAuthed ? auth.Token : null);
        }
        catch
        {
            services = new List<ServiceItemDto>();
        }

        return View(services);
    }
}