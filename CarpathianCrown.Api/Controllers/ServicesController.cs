using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarpathianCrown.Api.Data;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController(AppDbContext db) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string lang = "ua")
    {
        var isEn = lang.ToLowerInvariant() == "en";

        var items = await db.ServiceItems
            .Where(x => x.IsActive && x.Category == "Service")
            .OrderBy(x => x.Id)
            .ToListAsync();

        return Ok(items.Select(x => new
        {
            x.Id,
            x.Category,
            NameUa = x.NameUa,
            NameEn = x.NameEn,
            DescriptionUa = x.DescriptionUa,
            DescriptionEn = x.DescriptionEn,
            x.Price,
            x.IsActive,
            x.ImageUrl
        }));
    }

    [AllowAnonymous]
    [HttpGet("restaurant")]
    public async Task<IActionResult> Restaurant([FromQuery] string lang = "ua")
    {
        var isEn = lang.ToLowerInvariant() == "en";

        var items = await db.ServiceItems
            .Where(x => x.IsActive && x.Category == "Restaurant")
            .OrderBy(x => x.Id)
            .ToListAsync();

        return Ok(items.Select(x => new
        {
            x.Id,
            x.Category,
            NameUa = x.NameUa,
            NameEn = x.NameEn,
            DescriptionUa = x.DescriptionUa,
            DescriptionEn = x.DescriptionEn,
            x.Price,
            x.ImageUrl
        }));
    }
}