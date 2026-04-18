using CarpathianCrown.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/content")]
public class HotelContentController(AppDbContext db) : ControllerBase
{
    // lang = ua/en
    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug, [FromQuery] string lang = "ua")
    {
        var p = await db.ContentPages.FirstOrDefaultAsync(x => x.Slug == slug);
        if (p is null) return NotFound();

        var isEn = lang.ToLowerInvariant() == "en";
        return Ok(new
        {
            p.Slug,
            Title = isEn ? p.TitleEn : p.TitleUa,
            Html = isEn ? p.HtmlEn : p.HtmlUa,
            p.HeroImageUrl
        });
    }
}