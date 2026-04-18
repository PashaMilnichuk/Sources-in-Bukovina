using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class AccountController : Controller
{
    private readonly ApiClient _api;
    public AccountController(ApiClient api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        var bookings = await _api.Get<List<BookingDto>>($"/api/bookings/mine?lang={lang}", auth.Token);
        var me = await _api.Get<MeDto>("/api/me", auth.Token);

        ViewBag.Auth = auth;
        ViewBag.Me = me;

        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> NewBooking(int? room, DateOnly? checkIn, DateOnly? checkOut)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        ViewBag.Rooms = await _api.Get<List<RoomDto>>(
            $"/api/rooms?lang={lang}",
            auth.Token);

        ViewBag.Services = await _api.Get<List<ServiceItemDto>>(
            $"/api/services?lang={lang}",
            auth.Token);

        ViewBag.Auth = auth;

        return View(new BookingCreateRequest
        {
            RoomId = room ?? 0,
            CheckIn = checkIn ?? DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            CheckOut = checkOut ?? DateOnly.FromDateTime(DateTime.Today.AddDays(2))
        });
    }

    [HttpPost]
    public async Task<IActionResult> NewBooking(BookingCreateRequest req)
    {
        var auth = HttpContext.GetAuth();

        if (auth.Role == "Admin")
            return Forbid();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        try
        {
            var booking = await _api.Post<BookingCreateRequest, BookingDetailsDto>(
                "/api/bookings",
                req,
                auth.Token);

            if (req.ServiceItemIds.Any())
            {
                await _api.Post("/api/bookings/services", new
                {
                    bookingId = booking.Id,
                    lines = req.ServiceItemIds.Select(x => new
                    {
                        serviceItemId = x,
                        quantity = 1
                    }).ToList()
                }, auth.Token);
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.Auth = auth;

            ViewBag.Rooms = await _api.Get<List<RoomDto>>(
                $"/api/rooms?lang={lang}",
                auth.Token);

            ViewBag.Services = await _api.Get<List<ServiceItemDto>>(
                $"/api/services?lang={lang}",
                auth.Token);

            return View(req);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        try
        {
            await _api.Post($"/api/bookings/{id}/cancel", new { }, auth.Token);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    private async Task LoadBookingLookups(string lang, string? token)
    {
        ViewBag.Rooms = await _api.Get<List<RoomDto>>($"/api/rooms?lang={lang}", token);
        ViewBag.Services = await _api.Get<List<ServiceItemDto>>($"/api/services?lang={lang}", token);
    }
    [HttpGet]
    public async Task<IActionResult> Booking(int id)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();
        var dto = await _api.Get<BookingDetailsDto>($"/api/bookings/{id}?lang={lang}", auth.Token);

        ViewBag.Auth = auth;
        return View(dto);
    }

    [HttpGet]
    public IActionResult Review(int id)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        ViewBag.Auth = auth;
        return View(id);
    }

    [HttpPost]
    public async Task<IActionResult> Review(int bookingId, int rating, string comment)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        ViewBag.Auth = auth;

        if (bookingId <= 0)
        {
            ViewBag.Error = "Invalid booking";
            return View(bookingId);
        }

        if (rating < 1 || rating > 5)
        {
            ViewBag.Error = "Rating must be between 1 and 5";
            return View(bookingId);
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            ViewBag.Error = "Поле Коментар порожнє";
            return View(bookingId);
        }

        try
        {
            await _api.Post("/api/reviews", new
            {
                bookingId,
                rating,
                comment
            }, auth.Token);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(bookingId);
        }
    }

    [HttpPost("/Account/Cancel/{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        try
        {
            await _api.Delete($"/api/bookings/{id}", auth.Token);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        ViewBag.Auth = auth;
        return View(new ChangePasswordVm());
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        ViewBag.Auth = auth;

        if (string.IsNullOrWhiteSpace(model.CurrentPassword) ||
            string.IsNullOrWhiteSpace(model.NewPassword) ||
            string.IsNullOrWhiteSpace(model.ConfirmPassword))
        {
            ViewBag.Error = "Усі поля обов'язкові";
            return View(model);
        }

        if (model.NewPassword != model.ConfirmPassword)
        {
            ViewBag.Error = "Нові паролі не співпадають";
            return View(model);
        }

        try
        {
            await _api.Post("/api/me/change-password", new
            {
                currentPassword = model.CurrentPassword,
                newPassword = model.NewPassword
            }, auth.Token);

            TempData["Success"] = "Ваш пароль змінено";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult DeleteAccount()
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        ViewBag.Auth = auth;
        return View(new DeleteAccountVm());
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAccount(DeleteAccountVm model)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        ViewBag.Auth = auth;

        if (string.IsNullOrWhiteSpace(model.Password))
        {
            ViewBag.Error = "Введіть пароль";
            return View(model);
        }

        try
        {
            await _api.DeleteWithBody("/api/me", new
            {
                password = model.Password
            }, auth.Token);

            HttpContext.Session.Clear();

            Response.Cookies.Delete("auth");
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> OrderToRoom(int bookingId)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        var booking = await _api.Get<BookingDetailsDto>(
            $"/api/bookings/{bookingId}?lang={lang}",
            auth.Token);

        var services = await _api.Get<List<ServiceItemDto>>(
            $"/api/services?lang={lang}",
            auth.Token);

        ViewBag.Auth = auth;
        ViewBag.Booking = booking;
        ViewBag.Services = services;

        return View(new CreateRoomOrderVm
        {
            BookingId = bookingId
        });
    }

    [HttpPost]
    public async Task<IActionResult> OrderToRoom(CreateRoomOrderVm model)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        try
        {
            var lines = model.Lines
                .Where(x => x.Quantity > 0)
                .Select(x => new
                {
                    serviceItemId = x.ServiceItemId,
                    quantity = x.Quantity
                })
                .ToList();

            if (!lines.Any())
                throw new InvalidOperationException("Choose at least one item");

            await _api.Post("/api/room-orders", new
            {
                bookingId = model.BookingId,
                lines
            }, auth.Token);

            TempData["Success"] = "Замовлення в номер створено";
            return RedirectToAction("RoomOrders");
        }
        catch (Exception ex)
        {
            var booking = await _api.Get<BookingDetailsDto>(
                $"/api/bookings/{model.BookingId}?lang={lang}",
                auth.Token);

            var services = await _api.Get<List<ServiceItemDto>>(
                $"/api/services?lang={lang}",
                auth.Token);

            ViewBag.Auth = auth;
            ViewBag.Booking = booking;
            ViewBag.Services = services;
            ViewBag.Error = ex.Message;

            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RoomOrders()
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        var orders = await _api.Get<List<RoomOrderDto>>(
            $"/api/room-orders/mine?lang={lang}",
            auth.Token);

        ViewBag.Auth = auth;
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> OrderCatalog(string category)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        var activeBookings = await _api.Get<List<ActiveBookingDto>>(
            $"/api/bookings/active?lang={lang}",
            auth.Token);

        var allServices = await _api.Get<List<ServiceItemDto>>(
            $"/api/services?lang={lang}",
            auth.Token);

        var items = string.Equals(category, "restaurant", StringComparison.OrdinalIgnoreCase)
            ? allServices.Where(x => string.Equals(x.Category, "Restaurant", StringComparison.OrdinalIgnoreCase)).ToList()
            : allServices.Where(x => !string.Equals(x.Category, "Restaurant", StringComparison.OrdinalIgnoreCase)).ToList();

        ViewBag.Auth = auth;
        ViewBag.Category = category;
        ViewBag.Items = items;
        ViewBag.ActiveBookings = activeBookings;

        return View(new CreateRoomOrderVm());
    }

    [HttpPost]
    public async Task<IActionResult> OrderCatalog(CreateRoomOrderVm model, string category)
    {
        var auth = HttpContext.GetAuth();
        if (!auth.IsAuthed) return this.RequireLogin();

        var lang = HttpContext.GetLang();

        try
        {
            var lines = model.Lines
                .Where(x => x.Quantity > 0)
                .Select(x => new
                {
                    serviceItemId = x.ServiceItemId,
                    quantity = x.Quantity
                })
                .ToList();

            if (model.BookingId <= 0)
                throw new InvalidOperationException("Оберіть бронювання");

            if (!lines.Any())
                throw new InvalidOperationException("Оберіть хоча б одну позицію");

            await _api.Post("/api/room-orders", new
            {
                bookingId = model.BookingId,
                lines
            }, auth.Token);

            TempData["Success"] = "Замовлення створено";
            return RedirectToAction("RoomOrders");
        }
        catch (Exception ex)
        {
            var activeBookings = await _api.Get<List<ActiveBookingDto>>(
                $"/api/bookings/active?lang={lang}",
                auth.Token);

            var allServices = await _api.Get<List<ServiceItemDto>>(
                $"/api/services?lang={lang}",
                auth.Token);

            var items = string.Equals(category, "restaurant", StringComparison.OrdinalIgnoreCase)
                ? allServices.Where(x => string.Equals(x.Category, "Restaurant", StringComparison.OrdinalIgnoreCase)).ToList()
                : allServices.Where(x => !string.Equals(x.Category, "Restaurant", StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.Auth = auth;
            ViewBag.Category = category;
            ViewBag.Items = items;
            ViewBag.ActiveBookings = activeBookings;
            ViewBag.Error = ex.Message;

            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> QuickOrder(int serviceItemId, int quantity)
    {
        var auth = HttpContext.GetAuth();

        if (!auth.IsAuthed) return this.RequireLogin();
        if (auth.Role == "Admin") return Forbid();

        if (quantity < 1) quantity = 1;

        var lang = HttpContext.GetLang();

        var bookings = await _api.Get<List<ActiveBookingDto>>(
            $"/api/bookings/active?lang={lang}",
            auth.Token);

        var booking = bookings.FirstOrDefault();

        if (booking == null)
        {
            TempData["Error"] = "Спочатку потрібно забронювати номер";
            return Redirect("/Rooms");
        }

        await _api.Post("/api/room-orders", new
        {
            bookingId = booking.Id,
            lines = new[]
            {
            new { serviceItemId, quantity }
        }
        }, auth.Token);

        TempData["Success"] = "Замовлення створено";

        return Redirect(Request.Headers["Referer"].ToString());
    }
}