using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/admin/content")]
[Authorize(Roles = "Admin")]
public class AdminContentController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var pages = await db.ContentPages.OrderBy(x => x.Slug).ToListAsync();
        return Ok(pages.Select(p => new {
            p.Id, p.Slug, p.TitleUa, p.TitleEn, p.HeroImageUrl
        }));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var p = await db.ContentPages.FirstOrDefaultAsync(x => x.Slug == slug);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPut]
    public async Task<IActionResult> Upsert(UpsertContentPageRequest req)
    {
        var slug = req.Slug.Trim().ToLowerInvariant();
        var p = await db.ContentPages.FirstOrDefaultAsync(x => x.Slug == slug);

        if (p is null)
        {
            p = new ContentPage { Slug = slug };
            db.ContentPages.Add(p);
        }

        p.TitleUa = req.TitleUa ?? "";
        p.TitleEn = req.TitleEn ?? "";
        p.HtmlUa = req.HtmlUa ?? "";
        p.HtmlEn = req.HtmlEn ?? "";
        p.HeroImageUrl = req.HeroImageUrl ?? "";

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var p = await db.ContentPages.FirstOrDefaultAsync(x => x.Slug == slug);
        if (p is null) return NotFound();
        db.ContentPages.Remove(p);
        await db.SaveChangesAsync();
        return Ok();
    }
}