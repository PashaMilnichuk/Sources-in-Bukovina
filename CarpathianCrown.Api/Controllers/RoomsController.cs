using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController(AppDbContext db, BookingDomainService domain) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string lang = "ua")
    {
        var rooms = await db.Rooms
            .OrderBy(r => r.Id)
            .Select(r => new
            {
                r.Id,
                r.NameUa,
                r.NameEn,
                r.DescriptionUa,
                r.DescriptionEn,
                r.PricePerNight,
                r.Capacity,
                r.Status,
                r.CoverImageUrl,
                r.Image2,
                r.Image3,
                r.Image4,
                AvgRating = db.Reviews
                    .Where(rv => rv.Booking.RoomId == r.Id)
                    .Select(rv => (double?)rv.Rating)
                    .Average() ?? 0
            })
            .ToListAsync();

        return Ok(rooms);
    }

    [HttpGet("available")]
    public async Task<IActionResult> Available(
    [FromQuery] DateOnly checkIn,
    [FromQuery] DateOnly checkOut,
    [FromQuery] string lang = "ua")
    {
        var rooms = await db.Rooms
            .Where(r => r.Status == "Available")
            .ToListAsync();

        var result = new List<object>();

        foreach (var r in rooms)
        {
            if (await domain.IsRoomAvailable(r.Id, checkIn, checkOut))
            {
                var avgRating = await db.Reviews
                    .Where(rv => rv.Booking.RoomId == r.Id)
                    .Select(rv => (double?)rv.Rating)
                    .AverageAsync() ?? 0;

                result.Add(new
                {
                    r.Id,
                    r.NameUa,
                    r.NameEn,
                    r.DescriptionUa,
                    r.DescriptionEn,
                    r.PricePerNight,
                    r.Capacity,
                    r.Status,
                    r.CoverImageUrl,
                    r.Image2,
                    r.Image3,
                    r.Image4,
                    AvgRating = avgRating
                });
            }
        }

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, [FromQuery] string lang = "ua")
    {
        var r = await db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
        if (r is null) return NotFound();

        var avgRating = await db.Reviews
            .Where(rv => rv.Booking.RoomId == r.Id)
            .Select(rv => (double?)rv.Rating)
            .AverageAsync() ?? 0;

        return Ok(new
        {
            r.Id,
            r.NameUa,
            r.NameEn,
            r.DescriptionUa,
            r.DescriptionEn,
            r.PricePerNight,
            r.Capacity,
            r.Status,
            r.CoverImageUrl,
            r.Image2,
            r.Image3,
            r.Image4,
            AvgRating = avgRating
        });
    }
}