using System.Security.Claims;
using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController(AppDbContext db) : ControllerBase
{
    [HttpGet("room/{roomId:int}")]
    public async Task<IActionResult> ForRoom(int roomId)
    {
        var list = await db.Reviews
            .Include(r => r.Booking)
            .Where(r => r.Booking.RoomId == roomId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new
            {
                r.Rating,
                r.Comment,
                r.CreatedAt
            })
            .ToListAsync();

        return Ok(list);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewRequest req)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId))
            return Unauthorized();

        if (req.BookingId <= 0)
            return BadRequest("Invalid booking");

        if (req.Rating < 1 || req.Rating > 5)
            return BadRequest("Rating must be between 1 and 5");

        if (string.IsNullOrWhiteSpace(req.Comment))
            return BadRequest("Comment is required");

        var booking = await db.Bookings
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b => b.Id == req.BookingId);

        if (booking == null)
            return BadRequest("Invalid booking");

        if (booking.UserId != userId)
            return BadRequest("Invalid booking");

        if (booking.Review != null)
            return Conflict("Review already exists");

        var allowedStatuses = new[] { "Confirmed", "Completed" };
        if (!allowedStatuses.Contains(booking.Status))
            return BadRequest("Review is allowed only for confirmed or completed booking");

        var review = new Review
        {
            BookingId = booking.Id,
            UserId = userId,
            Rating = req.Rating,
            Comment = req.Comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        db.Reviews.Add(review);
        await db.SaveChangesAsync();

        return Ok(new { message = "Review created" });
    }
}