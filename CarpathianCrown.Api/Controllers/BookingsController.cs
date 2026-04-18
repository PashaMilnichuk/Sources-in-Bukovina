using System.Security.Claims;
using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Services;
using CarpathianCrown.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingsController(AppDbContext db, BookingDomainService domain) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest req, [FromQuery] string lang = "ua")
    {
        if (User.IsInRole("Admin"))
            return Forbid();

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdStr, out var userId))
            return Unauthorized();

        try
        {
            var booking = await domain.CreateBooking(userId, req.RoomId, req.CheckIn, req.CheckOut);

            booking = await db.Bookings
                .Include(x => x.Room)
                .FirstAsync(x => x.Id == booking.Id);

            return Ok(ToDto(booking, lang));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = "Conflict", detail = ex.Message });
        }
    }

    [HttpPost("services")]
    public async Task<IActionResult> AddServices(AddServicesRequest req)
    {
        var lines = req.Lines.Select(x => (x.ServiceItemId, x.Quantity));
        await domain.AddServices(UserId, req.BookingId, lines);
        return Ok();
    }

    [HttpGet("mine")]
    public async Task<IActionResult> Mine([FromQuery] string lang = "ua")
    {
        var list = await db.Bookings
            .Where(b => b.UserId == UserId)
            .Include(b => b.Room)
            .Include(b => b.BookingServices).ThenInclude(x => x.ServiceItem)
            .Include(b => b.Review)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(list.Select(b => ToDto(b, lang)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await db.Bookings
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == UserId);

        if (booking == null)
            return NotFound(new { message = "Booking not found" });

        if (booking.Status == "Cancelled")
            return BadRequest(new { message = "Booking already cancelled" });

        if (booking.Status == "Completed")
            return BadRequest(new { message = "Completed booking cannot be cancelled" });

        var checkInUtc = DateTime.SpecifyKind(
            booking.CheckIn.ToDateTime(TimeOnly.MinValue),
            DateTimeKind.Utc);

        if (checkInUtc <= DateTime.UtcNow.AddHours(24))
            return BadRequest(new { message = "Booking can be cancelled no later than 24 hours before check-in" });

        booking.Status = "Cancelled";
        await db.SaveChangesAsync();

        return Ok(new
        {
            booking.Id,
            booking.Status
        });
    }

    private static object ToDto(CarpathianCrown.Api.Models.Booking b, string lang)
    {
        var title = lang == "en" ? b.Room?.NameEn : b.Room?.NameUa;

        return new
        {
            id = b.Id,
            roomId = b.RoomId,
            roomTitle = title ?? "",
            checkIn = b.CheckIn,
            checkOut = b.CheckOut,
            status = b.Status,
            grandTotal = b.GrandTotal,
            createdAt = b.CreatedAt,
            review = b.Review == null ? null : new
            {
                rating = b.Review.Rating,
                comment = b.Review.Comment,
                createdAt = b.Review.CreatedAt
            }
        };
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id, [FromQuery] string lang = "ua")
    {
        var booking = await db.Bookings
            .Include(b => b.Room)
            .Include(b => b.BookingServices).ThenInclude(x => x.ServiceItem)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == UserId);

        if (booking == null) return NotFound();

        var isEn = lang == "en";

        return Ok(new
        {
            booking.Id,
            booking.Status,
            booking.CheckIn,
            booking.CheckOut,
            booking.RoomTotal,
            booking.ServicesTotal,
            booking.GrandTotal,
            booking.CreatedAt,
            RoomTitle = isEn ? booking.Room.NameEn : booking.Room.NameUa,
            Services = booking.BookingServices.Select(x => new
            {
                x.ServiceItemId,
                Title = isEn ? x.ServiceItem.NameEn : x.ServiceItem.NameUa,
                x.Quantity,
                x.LineTotal
            }).ToList()
        });
    }

    [HttpGet("active")]
    public async Task<IActionResult> Active([FromQuery] string lang = "ua")
    {
        var isEn = lang == "en";

        var list = await db.Bookings
            .Include(b => b.Room)
            .Where(b => b.UserId == UserId && (b.Status == "Confirmed" || b.Status == "Completed"))
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(list.Select(b => new
        {
            b.Id,
            RoomTitle = isEn ? b.Room.NameEn : b.Room.NameUa,
            b.Status,
            b.CheckIn,
            b.CheckOut,
            b.CreatedAt
        }));
    }
}