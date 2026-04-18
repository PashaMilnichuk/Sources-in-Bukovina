using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarpathianCrown.Api.Data;
using System.Text.Json;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("bookings")]
    public async Task<IActionResult> GetBookings(
        [FromQuery] string? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 200) pageSize = 200;

        var q = _db.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            q = q.Where(b => b.Status == status);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new
            {
                b.Id,
                b.Status,
                b.CheckIn,
                b.CheckOut,
                b.RoomTotal,
                b.ServicesTotal,
                b.GrandTotal,
                b.CreatedAt,
                user = new { b.UserId, b.User.Email, b.User.Login },
                room = new { b.RoomId, b.Room.NameUa, b.Room.NameEn, b.Room.Capacity }
            })
            .ToListAsync();

        return Ok(new { total, page, pageSize, items });
    }

    [HttpPut("bookings/{id:int}/status")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateStatusRequest req)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return NotFound(new { message = "Booking not found" });

        var allowed = new HashSet<string> { "Pending", "Confirmed", "Completed", "Cancelled" };
        if (string.IsNullOrWhiteSpace(req.Status) || !allowed.Contains(req.Status))
            return BadRequest(new { message = "Invalid status" });

        booking.Status = req.Status;
        await _db.SaveChangesAsync();

        return Ok(new { booking.Id, booking.Status });
    }

    public record UpdateStatusRequest(string Status);

    [HttpGet("availability")]
    public async Task<IActionResult> Availability([FromQuery] DateOnly checkIn, [FromQuery] DateOnly checkOut)
    {
        if (checkOut <= checkIn)
            return BadRequest(new { message = "checkOut must be after checkIn" });

        var busyRoomIds = await _db.Bookings
            .Where(b => b.Status != "Cancelled"
                        && b.CheckIn < checkOut
                        && checkIn < b.CheckOut)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync();

        var rooms = await _db.Rooms
            .OrderBy(r => r.Id)
            .Select(r => new
            {
                r.Id,
                r.NameUa,
                r.NameEn,
                r.Capacity,
                r.PricePerNight,
                r.Status,
                isBusy = busyRoomIds.Contains(r.Id)
            })
            .ToListAsync();

        return Ok(new
        {
            checkIn,
            checkOut,
            totalRooms = rooms.Count,
            busy = rooms.Count(r => r.isBusy),
            free = rooms.Count(r => !r.isBusy),
            rooms
        });
    }

    [HttpGet("reports/revenue-daily")]
    public async Task<IActionResult> RevenueDaily([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        if (to < from)
            return BadRequest(new { message = "to must be >= from" });

        var fromDt = DateTime.SpecifyKind(
            from.ToDateTime(TimeOnly.MinValue),
            DateTimeKind.Utc);

        var toDtExclusive = DateTime.SpecifyKind(
            to.AddDays(1).ToDateTime(TimeOnly.MinValue),
            DateTimeKind.Utc);

        var rows = await _db.Bookings
            .Where(b => b.Status != "Cancelled"
                        && b.CreatedAt >= fromDt
                        && b.CreatedAt < toDtExclusive)
            .GroupBy(b => DateOnly.FromDateTime(b.CreatedAt))
            .Select(g => new
            {
                day = g.Key,
                bookings = g.Count(),
                revenue = g.Sum(x => x.GrandTotal)
            })
            .OrderBy(x => x.day)
            .ToListAsync();

        var result = rows.Select(x => new
        {
            label = x.day.ToString("yyyy-MM-dd"),
            bookings = x.bookings,
            revenue = x.revenue
        });

        return Ok(result);
    }
    [HttpGet("bookings/{id:int}")]
    public async Task<IActionResult> GetBookingDetails(int id)
    {
        var booking = await _db.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .Include(b => b.BookingServices).ThenInclude(x => x.ServiceItem)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null) return NotFound();

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
            User = new
            {
                booking.UserId,
                booking.User.Email,
                booking.User.Login,
                booking.User.FirstName,
                booking.User.LastName,
                booking.User.Phone
            },
            Room = new
            {
                booking.RoomId,
                booking.Room.NameUa,
                booking.Room.NameEn,
                booking.Room.Capacity,
                booking.Room.PricePerNight
            },
            Services = booking.BookingServices.Select(x => new
            {
                x.ServiceItemId,
                x.Quantity,
                x.LineTotal,
                x.ServiceItem.NameUa,
                x.ServiceItem.NameEn
            }).ToList()
        });
    }
    [HttpGet("reports/top-rooms")]
    public async Task<IActionResult> TopRooms()
    {
        var rows = await _db.Bookings
            .Where(b => b.Status != "Cancelled")
            .GroupBy(b => new { b.RoomId, b.Room.NameUa, b.Room.NameEn })
            .Select(g => new
            {
                roomId = g.Key.RoomId,
                nameUa = g.Key.NameUa,
                nameEn = g.Key.NameEn,
                bookings = g.Count(),
                revenue = g.Sum(x => x.GrandTotal)
            })
            .OrderByDescending(x => x.bookings)
            .ThenByDescending(x => x.revenue)
            .Take(5)
            .ToListAsync();

        return Ok(rows);
    }
    [HttpGet("reports/occupancy")]
    public async Task<IActionResult> Occupancy([FromQuery] DateOnly? from = null, [FromQuery] DateOnly? to = null)
    {
        var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-30));
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow.Date);

        if (toDate < fromDate)
            return BadRequest(new { message = "to must be >= from" });

        var fromDt = DateTime.SpecifyKind(fromDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        var toDtExclusive = DateTime.SpecifyKind(toDate.AddDays(1).ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

        var totalRooms = await _db.Rooms.CountAsync();

        var bookings = await _db.Bookings
            .Where(b => b.Status != "Cancelled"
                        && b.CreatedAt >= fromDt
                        && b.CreatedAt < toDtExclusive)
            .CountAsync();

        var occupiedRoomIds = await _db.Bookings
            .Where(b => b.Status != "Cancelled"
                        && b.CreatedAt >= fromDt
                        && b.CreatedAt < toDtExclusive)
            .Select(b => b.RoomId)
            .Distinct()
            .CountAsync();

        var occupancyRate = totalRooms == 0
            ? 0
            : Math.Round((double)occupiedRoomIds / totalRooms * 100, 1);

        return Ok(new
        {
            from = fromDate,
            to = toDate,
            totalRooms,
            occupiedRooms = occupiedRoomIds,
            bookings,
            occupancyRate
        });
    }
    [HttpGet("reports/occupancy-calendar")]
    public async Task<IActionResult> OccupancyCalendar([FromQuery] DateOnly? start = null, [FromQuery] int days = 14)
    {
        if (days < 1) days = 14;
        if (days > 31) days = 31;

        var startDate = start ?? DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var endDate = startDate.AddDays(days);

        var rooms = await _db.Rooms
            .OrderBy(r => r.Id)
            .Select(r => new
            {
                r.Id,
                r.NameUa,
                r.NameEn,
                r.Status
            })
            .ToListAsync();

        var bookings = await _db.Bookings
            .Where(b => b.Status != "Cancelled"
                        && b.CheckIn < endDate
                        && startDate < b.CheckOut)
            .Select(b => new
            {
                b.RoomId,
                b.CheckIn,
                b.CheckOut,
                b.Status
            })
            .ToListAsync();

        var dates = Enumerable.Range(0, days)
            .Select(offset => startDate.AddDays(offset))
            .ToList();

        var result = rooms.Select(room => new
        {
            room.Id,
            room.NameUa,
            room.NameEn,
            room.Status,
            Days = dates.Select(d => new
            {
                Date = d,
                IsBusy = bookings.Any(b =>
                    b.RoomId == room.Id &&
                    b.CheckIn <= d &&
                    d < b.CheckOut),
            }).ToList()
        });

        return Ok(new
        {
            Start = startDate,
            Days = days,
            Dates = dates,
            Rooms = result
        });
    }

    [HttpGet("reports/revenue-hourly")]
    public async Task<IActionResult> RevenueHourly()
    {
        var now = DateTime.UtcNow;
        var from = now.AddHours(-24);

        var bookings = await _db.Bookings
            .Where(b => b.Status != "Cancelled"
                        && b.CreatedAt >= from
                        && b.CreatedAt <= now)
            .OrderBy(b => b.CreatedAt)
            .Select(b => new
            {

                label = b.CreatedAt.ToString("HH:mm"),
                revenue = (double)b.GrandTotal
            })
            .ToListAsync();


        if (!bookings.Any())
        {
            bookings.Add(new { label = from.ToString("HH:mm"), revenue = 0.0 });
            bookings.Add(new { label = now.ToString("HH:mm"), revenue = 0.0 });
        }

        var result = new
        {
            items = bookings,
            maxRevenue = bookings.Max(x => x.revenue)
        };

        return Ok(result);
    }

}