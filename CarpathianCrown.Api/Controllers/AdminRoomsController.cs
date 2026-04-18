using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/admin/rooms")]
[Authorize(Roles = "Admin")]
public class AdminRoomsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var rooms = await db.Rooms.OrderBy(x => x.Id).ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room == null) return NotFound();
        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertRoomRequest req)
    {
        var room = new Room
        {
            NameUa = req.NameUa,
            NameEn = req.NameEn,
            DescriptionUa = req.DescriptionUa,
            DescriptionEn = req.DescriptionEn,
            PricePerNight = req.PricePerNight,
            Capacity = req.Capacity,
            Status = req.Status,
            CoverImageUrl = req.CoverImageUrl,
            Image2 = req.Image2,
            Image3 = req.Image3,
            Image4 = req.Image4
        };

        db.Rooms.Add(room);
        await db.SaveChangesAsync();
        return Ok(room.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertRoomRequest req)
    {
        var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room == null) return NotFound();

        room.NameUa = req.NameUa;
        room.NameEn = req.NameEn;
        room.DescriptionUa = req.DescriptionUa;
        room.DescriptionEn = req.DescriptionEn;
        room.PricePerNight = req.PricePerNight;
        room.Capacity = req.Capacity;
        room.Status = req.Status;
        room.CoverImageUrl = req.CoverImageUrl;
        room.Image2 = req.Image2;
        room.Image3 = req.Image3;
        room.Image4 = req.Image4;

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
        if (room == null) return NotFound();

        db.Rooms.Remove(room);
        await db.SaveChangesAsync();
        return Ok();
    }
}