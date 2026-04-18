using System.ComponentModel.DataAnnotations;

namespace CarpathianCrown.Api.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Email { get; set; } = "";

    [MaxLength(60)]
    public string? Login { get; set; }

    [Required]
    public string PasswordHash { get; set; } = "";

    [Required, MaxLength(20)]
    public string Role { get; set; } = "User";

    [MaxLength(80)]
    public string FirstName { get; set; } = "";

    [MaxLength(80)]
    public string LastName { get; set; } = "";

    [MaxLength(40)]
    public string? Phone { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Booking> Bookings { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}