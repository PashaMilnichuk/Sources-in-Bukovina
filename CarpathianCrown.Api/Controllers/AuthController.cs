using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.DTOs;
using CarpathianCrown.Api.Models;
using CarpathianCrown.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, JwtService jwt) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        var login = req.Login?.Trim();

        if (await db.Users.AnyAsync(u => u.Email == email))
            return Conflict("Email already exists.");

        if (!string.IsNullOrWhiteSpace(login) && await db.Users.AnyAsync(u => u.Login == login))
            return Conflict("Login already exists.");

        if (!req.AcceptPersonalData)
            return BadRequest("You must consent to the processing of your personal data.");

        var user = new User
        {
            Email = email,
            Login = string.IsNullOrWhiteSpace(login) ? null : login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Role = "User",
            FirstName = req.FirstName ?? "",
            LastName = req.LastName ?? "",
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var token = jwt.Generate(user);
        return Ok(new AuthResponse(token, user.Role, user.Id));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var identifier = req.Identifier.Trim();
        var user = await db.Users.FirstOrDefaultAsync(u =>
            u.Email == identifier.ToLowerInvariant() || u.Login == identifier);

        if (user is null) return Unauthorized("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = jwt.Generate(user);
        return Ok(new AuthResponse(token, user.Role, user.Id));
    }
}