namespace CarpathianCrown.Web.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class AdminBookingDto
{
    public int Id { get; set; }

    public JsonElement User { get; set; }
    public JsonElement Room { get; set; }

    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public string Status { get; set; } = "";
    public decimal GrandTotal { get; set; }
    public DateTime CreatedAt { get; set; }
    public double AvgRating { get; set; }

    [JsonIgnore]
    public string UserText => ExtractText(User, "email", "Email", "login", "Login", "fullName", "FullName");

    [JsonIgnore]
    public string RoomText => ExtractText(Room, "nameEn", "NameEn", "title", "Title", "nameUa", "NameUa");

    private static string ExtractText(JsonElement element, params string[] names)
    {
        if (element.ValueKind == JsonValueKind.String)
            return element.GetString() ?? "";

        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var name in names)
            {
                if (element.TryGetProperty(name, out var prop))
                {
                    if (prop.ValueKind == JsonValueKind.String)
                        return prop.GetString() ?? "";

                    if (prop.ValueKind == JsonValueKind.Number)
                        return prop.ToString();
                }
            }

            return element.ToString();
        }

        return element.ToString();
    }
}

public sealed class AuthResponse
{
    public string Token { get; set; } = "";
    public string Role { get; set; } = "";
    public int UserId { get; set; }
}

public sealed class RoomDto
{
    public int Id { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string DescriptionUa { get; set; } = "";
    public string DescriptionEn { get; set; } = "";
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = "";
    public string CoverImageUrl { get; set; } = "";
    public double AvgRating { get; set; }
    public string? Image2 { get; set; }
    public string? Image3 { get; set; }
    public string? Image4 { get; set; }
}

public sealed class ServiceItemDto
{
    public int Id { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string DescriptionUa { get; set; } = "";
    public string DescriptionEn { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "";
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
}

public sealed class BookingCreateRequest
{
    public int RoomId { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }

    public List<int> ServiceItemIds { get; set; } = new();
}

public sealed class BookingDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomTitle { get; set; } = "";
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public string Status { get; set; } = "";
    public decimal GrandTotal { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class ContentPageDto
{
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string Html { get; set; } = "";
    public string HeroImageUrl { get; set; } = "";
}

public sealed class AdminChartPointDto
{
    public string Label { get; set; } = "";
    public decimal Revenue { get; set; }
    public int Bookings { get; set; }
}
public sealed class AdminDashboardDto
{
    public int TotalBookings { get; set; }
    public int ActiveBookings { get; set; }
    public int CompletedBookings { get; set; }
    public decimal TotalRevenue { get; set; }
}
public sealed class AdminBookingsResponseDto
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<AdminBookingDto> Items { get; set; } = new();
}
public sealed class RevenueDailyDto
{
    public string Label { get; set; } = "";
    public int Bookings { get; set; }
    public decimal Revenue { get; set; }
}
public sealed class ReviewDto
{
    public int Rating { get; set; }
    public string Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
public sealed class BookingServiceLineDto
{
    public int ServiceItemId { get; set; }
    public string Title { get; set; } = "";
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
}

public sealed class BookingDetailsDto
{
    public int Id { get; set; }
    public string Status { get; set; } = "";
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public decimal RoomTotal { get; set; }
    public decimal ServicesTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime CreatedAt { get; set; }
    public string RoomTitle { get; set; } = "";
    public List<BookingServiceLineDto> Services { get; set; } = new();
}
public sealed class AdminBookingUserDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = "";
    public string Login { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";
}

public sealed class AdminBookingRoomDto
{
    public int RoomId { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
}

public sealed class AdminBookingServiceDto
{
    public int ServiceItemId { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
}

public sealed class AdminBookingDetailsDto
{
    public int Id { get; set; }
    public string Status { get; set; } = "";
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public decimal RoomTotal { get; set; }
    public decimal ServicesTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime CreatedAt { get; set; }
    public AdminBookingUserDto User { get; set; } = new();
    public AdminBookingRoomDto Room { get; set; } = new();
    public List<AdminBookingServiceDto> Services { get; set; } = new();
}
public sealed class AdminAvailabilityRoomDto
{
    public int Id { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public string Status { get; set; } = "";
    public bool IsBusy { get; set; }
}

public sealed class AdminAvailabilityDto
{
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int TotalRooms { get; set; }
    public int Busy { get; set; }
    public int Free { get; set; }
    public List<AdminAvailabilityRoomDto> Rooms { get; set; } = new();
}
public sealed class TopRoomDto
{
    public int RoomId { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public int Bookings { get; set; }
    public decimal Revenue { get; set; }
}

public sealed class OccupancyDto
{
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int Bookings { get; set; }
    public double OccupancyRate { get; set; }
}
public sealed class OccupancyCalendarDayDto
{
    public DateOnly Date { get; set; }
    public bool IsBusy { get; set; }
}

public sealed class OccupancyCalendarRoomDto
{
    public int Id { get; set; }
    public string NameUa { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string Status { get; set; } = "";
    public List<OccupancyCalendarDayDto> Days { get; set; } = new();
}

public sealed class OccupancyCalendarDto
{
    public DateOnly Start { get; set; }
    public int Days { get; set; }
    public List<DateOnly> Dates { get; set; } = new();
    public List<OccupancyCalendarRoomDto> Rooms { get; set; } = new();
}

public sealed class MeDto
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Role { get; set; } = "";
    public int BookingsCount { get; set; }
}

public sealed class ChangePasswordVm
{
    public string CurrentPassword { get; set; } = "";
    public string NewPassword { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
}

public sealed class DeleteAccountVm
{
    public string Password { get; set; } = "";
}

public sealed class RevenueHourlyDto
{
    public string Label { get; set; } = "";
    public int Bookings { get; set; }
    public decimal Revenue { get; set; }
}

public sealed class RoomOrderItemDto
{
    public int ServiceItemId { get; set; }
    public string Title { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public sealed class RoomOrderDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public string RoomTitle { get; set; } = "";
    public string Status { get; set; } = "";
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RoomOrderItemDto> Items { get; set; } = new();
}

public sealed class CreateRoomOrderVm
{
    public int BookingId { get; set; }
    public List<CreateRoomOrderLineVm> Lines { get; set; } = new();
}

public sealed class CreateRoomOrderLineVm
{
    public int ServiceItemId { get; set; }
    public int Quantity { get; set; }
}

public sealed class ActiveBookingDto
{
    public int Id { get; set; }
    public string RoomTitle { get; set; } = "";
    public string Status { get; set; } = "";
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public DateTime CreatedAt { get; set; }
}