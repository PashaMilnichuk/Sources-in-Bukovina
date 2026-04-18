using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/admin/services")]
[Authorize(Roles = "Admin")]
public class AdminServiceItemsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var items = await db.ServiceItems.OrderBy(x => x.Id).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await db.ServiceItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertServiceItemRequest req)
    {
        var item = new ServiceItem
        {
            Category = req.Category,
            NameUa = req.NameUa,
            NameEn = req.NameEn,
            DescriptionUa = req.DescriptionUa,
            DescriptionEn = req.DescriptionEn,
            Price = req.Price,
            IsActive = req.IsActive,
            ImageUrl = req.ImageUrl
        };

        db.ServiceItems.Add(item);
        await db.SaveChangesAsync();
        return Ok(item.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertServiceItemRequest req)
    {
        var item = await db.ServiceItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null) return NotFound();

        item.Category = req.Category;
        item.NameUa = req.NameUa;
        item.NameEn = req.NameEn;
        item.DescriptionUa = req.DescriptionUa;
        item.DescriptionEn = req.DescriptionEn;
        item.Price = req.Price;
        item.IsActive = req.IsActive;
        item.ImageUrl = req.ImageUrl;

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.ServiceItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null) return NotFound();

        db.ServiceItems.Remove(item);
        await db.SaveChangesAsync();
        return Ok();
    }
}