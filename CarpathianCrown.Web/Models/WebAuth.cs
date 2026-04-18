namespace CarpathianCrown.Web.Models;

public sealed record WebAuth(bool IsAuthed, string? Token, string? Role, string? Email, int? UserId);