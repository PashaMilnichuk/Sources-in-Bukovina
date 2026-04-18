using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/admin/room-orders")]
//[Authorize(Roles = "Admin")]
public class AdminRoomOrdersController(AppDbContext db) : ControllerBase
{
   //private readonly AppDbContext db;

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] string lang = "ua")
    {
        var isEn = lang == "en";

        var list = await db.RoomOrders
            .Include(x => x.User)
            .Include(x => x.Booking).ThenInclude(x => x.Room)
            .Include(x => x.Items).ThenInclude(x => x.ServiceItem)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(list.Select(o => new
        {
            o.Id,
            o.BookingId,
            UserEmail = o.User.Email,
            RoomTitle = isEn ? o.Booking.Room.NameEn : o.Booking.Room.NameUa,
            o.Status,
            o.Total,
            o.CreatedAt,
            Items = o.Items.Select(i => new
            {
                Title = isEn ? i.ServiceItem.NameEn : i.ServiceItem.NameUa,
                i.Quantity,
                i.LineTotal
            }).ToList()
        }));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string lang = "ua")
    {
        var isEn = lang == "en";

        var orders = await db.RoomOrders
            .Include(o => o.User)
            .Include(o => o.Booking)
                .ThenInclude(b => b.Room)
            .Include(o => o.Items)
                .ThenInclude(i => i.ServiceItem)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        var result = new List<object>();

        foreach (var o in orders)
        {
            var itemsList = new List<object>();
            decimal total = 0;

            if (o.Items != null)
            {
                foreach (var i in o.Items)
                {
                    if (i == null) continue;

                    var lineTotal = i.Quantity * i.UnitPrice;
                    total += lineTotal;

                    itemsList.Add(new
                    {
                        Title = i.ServiceItem != null
                            ? (isEn ? i.ServiceItem.NameEn : i.ServiceItem.NameUa)
                            : "—",

                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        LineTotal = lineTotal
                    });
                }
            }

            result.Add(new
            {
                o.Id,
                o.BookingId,

                UserEmail = o.User?.Email ?? "—",

                RoomTitle = o.Booking?.Room != null
                    ? (isEn ? o.Booking.Room.NameEn : o.Booking.Room.NameUa)
                    : "—",

                o.Status,
                o.CreatedAt,

                Total = total,
                Items = itemsList
            });
        }

        return Ok(result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] JsonElement body)
    {
        var order = await db.RoomOrders.FindAsync(id);
        if (order == null) return NotFound();

        if (!body.TryGetProperty("status", out var statusProp))
            return BadRequest("No status");

        var status = statusProp.GetString();

        var allowed = new[] { "New", "InProgress", "Completed", "Cancelled" };

        if (!allowed.Contains(status))
            return BadRequest("Invalid status");

        order.Status = status;

        await db.SaveChangesAsync();

        return Ok();
    }
}