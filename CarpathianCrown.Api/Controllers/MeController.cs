using System.Security.Claims;
using BCrypt.Net;
using CarpathianCrown.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/me")]
[Authorize]
public class MeController(AppDbContext db) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Id == UserId);

        if (user == null) return NotFound();

        var bookingsCount = await db.Bookings.CountAsync(b => b.UserId == UserId);

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.Role,
            bookingsCount
        });
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
        if (user == null) return NotFound();

        if (string.IsNullOrWhiteSpace(req.CurrentPassword) ||
            string.IsNullOrWhiteSpace(req.NewPassword))
            return BadRequest(new { message = "Password fields are required" });

        if (!BCrypt.Net.BCrypt.Verify(req.CurrentPassword, user.PasswordHash))
            return BadRequest(new { message = "Current password is incorrect" });

        if (req.NewPassword.Length < 6)
            return BadRequest(new { message = "New password must be at least 6 characters" });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
        await db.SaveChangesAsync();

        return Ok(new { message = "Password changed" });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest req)
    {
        var user = await db.Users
            .Include(u => u.Bookings)
            .ThenInclude(b => b.Review)
            .FirstOrDefaultAsync(u => u.Id == UserId);

        if (user == null) return NotFound();

        if (string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new { message = "Password is required" });

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return BadRequest(new { message = "Password is incorrect" });

        var activeBookings = await db.Bookings.AnyAsync(b =>
            b.UserId == UserId &&
            (b.Status == "Pending" || b.Status == "Confirmed"));

        if (activeBookings)
            return BadRequest(new { message = "Cancel active bookings before deleting account" });

        db.Users.Remove(user);
        await db.SaveChangesAsync();

        return Ok(new { message = "Account deleted" });
    }

    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
    public record DeleteAccountRequest(string Password);
}