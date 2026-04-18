using System.Security.Claims;
using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/room-orders")]
[Authorize]
public class RoomOrdersController(AppDbContext db) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("mine")]
    public async Task<IActionResult> Mine([FromQuery] string lang = "ua")
    {
        var isEn = lang == "en";

        var list = await db.RoomOrders
            .Where(x => x.UserId == UserId)
            .Include(x => x.Booking).ThenInclude(x => x.Room)
            .Include(x => x.Items).ThenInclude(x => x.ServiceItem)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(list.Select(o => new
        {
            o.Id,
            o.BookingId,
            RoomTitle = isEn ? o.Booking.Room.NameEn : o.Booking.Room.NameUa,
            o.Status,
            o.Total,
            o.CreatedAt,
            Items = o.Items.Select(i => new
            {
                i.ServiceItemId,
                Title = isEn ? i.ServiceItem.NameEn : i.ServiceItem.NameUa,
                i.Quantity,
                i.UnitPrice,
                i.LineTotal
            }).ToList()
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomOrderRequest req)
    {
        if (User.IsInRole("Admin"))
            return Forbid();

        if (req.Lines == null || req.Lines.Count == 0)
            return BadRequest(new { message = "Order lines are required" });

        var booking = await db.Bookings
            .FirstOrDefaultAsync(b => b.Id == req.BookingId && b.UserId == UserId);

        if (booking == null)
            return BadRequest(new { message = "Invalid booking" });

        if (booking.Status != "Confirmed" && booking.Status != "Completed")
            return BadRequest(new { message = "Room order allowed only for confirmed or completed booking" });

        var ids = req.Lines.Select(x => x.ServiceItemId).Distinct().ToList();

        var items = await db.ServiceItems
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync();

        if (items.Count != ids.Count)
            return BadRequest(new { message = "Some service items are invalid" });

        var order = new RoomOrder
        {
            BookingId = booking.Id,
            UserId = UserId,
            Status = "New",
            CreatedAt = DateTime.UtcNow
        };

        foreach (var line in req.Lines.Where(x => x.Quantity > 0))
        {
            var item = items.First(x => x.Id == line.ServiceItemId);

            order.Items.Add(new RoomOrderItem
            {
                ServiceItemId = item.Id,
                Quantity = line.Quantity,
                UnitPrice = item.Price,
                LineTotal = item.Price * line.Quantity
            });
        }

        order.Total = order.Items.Sum(x => x.LineTotal);

        db.RoomOrders.Add(order);
        await db.SaveChangesAsync();

        return Ok(new
        {
            order.Id,
            order.BookingId,
            order.Status,
            order.Total,
            order.CreatedAt
        });
    }
}