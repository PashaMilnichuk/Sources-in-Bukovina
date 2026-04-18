using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class AdminController : Controller
{
    private readonly ApiClient _api;

    public AdminController(ApiClient api)
    {
        _api = api;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var bookingsResponse = await _api.Get<AdminBookingsResponseDto>(
            "/api/admin/bookings",
            auth.Token);

        var revenueResponse = await _api.Get<RevenueHourlyResponse>(
            "/api/admin/reports/revenue-hourly",
            auth.Token);

        var revenueHourly = revenueResponse.Items;

        var bookings = bookingsResponse.Items;

        var totalBookings = bookingsResponse.Total;
        var activeBookings = bookings.Count(x => x.Status == "Pending" || x.Status == "Confirmed");
        var completedBookings = bookings.Count(x => x.Status == "Completed");
        var totalRevenue = bookings
            .Where(x => x.Status == "Completed")
            .Sum(x => x.GrandTotal);

        ViewBag.Auth = auth;
        ViewBag.Chart = revenueHourly;
        ViewBag.Chart = revenueResponse.Items;
        ViewBag.MaxRevenue = revenueResponse.MaxRevenue;

        var topRooms = await _api.Get<List<TopRoomDto>>(
            "/api/admin/reports/top-rooms",
            auth.Token);

        var occupancy = await _api.Get<OccupancyDto>(
            "/api/admin/reports/occupancy",
            auth.Token);

        ViewBag.TopRooms = topRooms;
        ViewBag.Occupancy = occupancy;

        return View(new AdminDashboardDto
        {
            TotalBookings = totalBookings,
            ActiveBookings = activeBookings,
            CompletedBookings = completedBookings,
            TotalRevenue = totalRevenue
        });
    }

    [HttpGet]
    public async Task<IActionResult> Bookings(string? status = null)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var url = "/api/admin/bookings";
        if (!string.IsNullOrWhiteSpace(status))
            url += $"?status={Uri.EscapeDataString(status)}";

        var response = await _api.Get<AdminBookingsResponseDto>(url, auth.Token);

        ViewBag.Auth = auth;
        ViewBag.Status = status;

        return View(response.Items);
    }

    [HttpPost]
    public async Task<IActionResult> Status(int id, string status)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Put(
            $"/api/admin/bookings/{id}/status",
            new { status },
            auth.Token);

        return RedirectToAction("Bookings");
    }

    [HttpGet]
    public async Task<IActionResult> BookingDetails(int id)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var dto = await _api.Get<AdminBookingDetailsDto>($"/api/admin/bookings/{id}", auth.Token);

        ViewBag.Auth = auth;
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Rooms()
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var rooms = await _api.Get<List<RoomDto>>("/api/admin/rooms", auth.Token);

        ViewBag.Auth = auth;
        return View(rooms);
    }

    [HttpGet]
    public IActionResult CreateRoom()
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        ViewBag.Auth = auth;
        return View("RoomForm", new AdminRoomFormVm());
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(AdminRoomFormVm vm)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Post("/api/admin/rooms", new
        {
            vm.NameUa,
            vm.NameEn,
            vm.DescriptionUa,
            vm.DescriptionEn,
            vm.PricePerNight,
            vm.Capacity,
            vm.Status,
            vm.CoverImageUrl,
            vm.Image2,
            vm.Image3,
            vm.Image4
        }, auth.Token);

        return RedirectToAction("Rooms");
    }

    [HttpGet]
    public async Task<IActionResult> EditRoom(int id)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var room = await _api.Get<AdminRoomFormVm>($"/api/admin/rooms/{id}", auth.Token);

        ViewBag.Auth = auth;
        return View("RoomForm", room);
    }

    [HttpPost]
    public async Task<IActionResult> EditRoom(AdminRoomFormVm vm)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Put($"/api/admin/rooms/{vm.Id}", new
        {
            vm.NameUa,
            vm.NameEn,
            vm.DescriptionUa,
            vm.DescriptionEn,
            vm.PricePerNight,
            vm.Capacity,
            vm.Status,
            vm.CoverImageUrl,
            vm.Image2,
            vm.Image3,
            vm.Image4
        }, auth.Token);

        return RedirectToAction("Rooms");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Delete($"/api/admin/rooms/{id}", auth.Token);
        return RedirectToAction("Rooms");
    }

    [HttpGet]
    public async Task<IActionResult> Services()
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var services = await _api.Get<List<ServiceItemDto>>("/api/admin/services", auth.Token);

        ViewBag.Auth = auth;
        return View(services);
    }

    [HttpGet]
    public IActionResult CreateService()
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        ViewBag.Auth = auth;
        return View("ServiceForm", new AdminServiceFormVm());
    }

    [HttpPost]
    public async Task<IActionResult> CreateService(AdminServiceFormVm vm)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Post("/api/admin/services", new
        {
            vm.Category,
            vm.NameUa,
            vm.NameEn,
            vm.DescriptionUa,
            vm.DescriptionEn,
            vm.Price,
            vm.IsActive,
            vm.ImageUrl
        }, auth.Token);

        return RedirectToAction("Services");
    }

    [HttpGet]
    public async Task<IActionResult> EditService(int id)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var service = await _api.Get<AdminServiceFormVm>($"/api/admin/services/{id}", auth.Token);

        ViewBag.Auth = auth;
        return View("ServiceForm", service);
    }

    [HttpPost]
    public async Task<IActionResult> EditService(AdminServiceFormVm vm)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Put($"/api/admin/services/{vm.Id}", new
        {
            vm.Category,
            vm.NameUa,
            vm.NameEn,
            vm.DescriptionUa,
            vm.DescriptionEn,
            vm.Price,
            vm.IsActive,
            vm.ImageUrl
        }, auth.Token);

        return RedirectToAction("Services");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteService(int id)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Delete($"/api/admin/services/{id}", auth.Token);
        return RedirectToAction("Services");
    }

    [HttpGet]
    public async Task<IActionResult> Availability(DateOnly? start, int? days)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var startDate = start ?? DateOnly.FromDateTime(DateTime.Today);
        var daysCount = days ?? 14;

        var dto = await _api.Get<OccupancyCalendarDto>(
            $"/api/admin/reports/occupancy-calendar?start={startDate:yyyy-MM-dd}&days={daysCount}",
            auth.Token);

        ViewBag.Auth = auth;
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> RoomOrders()
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        var lang = HttpContext.GetLang();

        var orders = await _api.Get<List<RoomOrderDto>>(
            $"/api/admin/room-orders?lang={lang}",
            auth.Token);

        ViewBag.Auth = auth;
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoomOrderStatus(int id, string status)
    {
        var auth = HttpContext.GetAuth();
        if (auth.Role != "Admin") return Redirect("/");

        await _api.Put($"/api/admin/room-orders/{id}/status", new { status }, auth.Token);

        return RedirectToAction("RoomOrders");
    }
}