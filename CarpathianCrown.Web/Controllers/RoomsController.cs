using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class RoomsController : Controller
{
    private readonly ApiClient _api;

    public RoomsController(ApiClient api)
    {
        _api = api;
    }

    [HttpGet("/Rooms")]
    public async Task<IActionResult> Index(DateOnly? checkIn, DateOnly? checkOut, int? guests)
    {
        var auth = HttpContext.GetAuth();
        var lang = HttpContext.GetLang();

        List<RoomDto> rooms;

        if (checkIn.HasValue && checkOut.HasValue)
        {
            rooms = await _api.Get<List<RoomDto>>(
                $"/api/rooms/available?checkIn={checkIn:yyyy-MM-dd}&checkOut={checkOut:yyyy-MM-dd}&lang={lang}");
        }
        else
        {
            rooms = await _api.Get<List<RoomDto>>($"/api/rooms?lang={lang}");
        }

        if (guests.HasValue)
            rooms = rooms.Where(r => r.Capacity >= guests.Value).ToList();

        ViewBag.Auth = auth;
        ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd");
        ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd");
        ViewBag.Guests = guests;

        return View(rooms);
    }

    [HttpGet("/Rooms/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var auth = HttpContext.GetAuth();
        var lang = HttpContext.GetLang();

        var room = await _api.Get<RoomDto>($"/api/rooms/{id}?lang={lang}");

        List<ServiceItemDto> services;
        if (auth.IsAuthed)
        {
            services = await _api.Get<List<ServiceItemDto>>(
                $"/api/services?lang={lang}",
                auth.Token);
        }
        else
        {
            services = new List<ServiceItemDto>();
        }

        var reviews = await _api.Get<List<ReviewDto>>($"/api/reviews/room/{id}");
        var avg = reviews.Any() ? Math.Round(reviews.Average(x => x.Rating), 1) : 0;

        ViewBag.Auth = auth;
        ViewBag.Services = services;
        ViewBag.Reviews = reviews;
        ViewBag.AvgRating = avg;

        return View(room);
    }
}