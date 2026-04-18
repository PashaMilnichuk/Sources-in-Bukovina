using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Services;

public class BookingDomainService(AppDbContext db)
{
    public static int Nights(DateOnly inDate, DateOnly outDate)
        => outDate.DayNumber - inDate.DayNumber;

    public async Task<bool> IsRoomAvailable(int roomId, DateOnly checkIn, DateOnly checkOut)
    {
        return !await db.Bookings
            .Where(b => b.RoomId == roomId)
            .Where(b => b.Status != "Cancelled")
            .AnyAsync(b => checkIn < b.CheckOut && b.CheckIn < checkOut);
    }

    public async Task<Booking> CreateBooking(int userId, int roomId, DateOnly checkIn, DateOnly checkOut)
    {
        if (checkOut <= checkIn) throw new InvalidOperationException("CheckOut must be after CheckIn.");

        var room = await db.Rooms.FirstAsync(r => r.Id == roomId);

        var status = (room.Status ?? "").Trim();
        if (!string.Equals(status, "Available", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Room not available (status).");

        var ok = await IsRoomAvailable(roomId, checkIn, checkOut);
        if (!ok) throw new InvalidOperationException("Room not available for selected dates.");

        var nights = Nights(checkIn, checkOut);
        if (nights <= 0) throw new InvalidOperationException("Invalid nights.");

        var roomTotal = room.PricePerNight * nights;

        var booking = new Booking
        {
            UserId = userId,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            Status = "Pending",
            RoomTotal = roomTotal,
            ServicesTotal = 0,
            GrandTotal = roomTotal
        };

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();
        return booking;
    }

    public async Task AddServices(int userId, int bookingId, IEnumerable<(int serviceId, int qty)> lines)
    {
        var booking = await db.Bookings
            .Include(b => b.BookingServices)
            .FirstAsync(b => b.Id == bookingId);

        if (booking.UserId != userId) throw new UnauthorizedAccessException();
        if (booking.Status is "Cancelled" or "Completed") throw new InvalidOperationException("Booking closed.");

        foreach (var (serviceId, qtyRaw) in lines)
        {
            var qty = Math.Max(1, qtyRaw);
            var svc = await db.ServiceItems.FirstAsync(s => s.Id == serviceId && s.IsActive);

            var line = new BookingService
            {
                BookingId = bookingId,
                ServiceItemId = serviceId,
                Quantity = qty,
                UnitPrice = svc.Price,
                LineTotal = svc.Price * qty
            };
            db.BookingServices.Add(line);
        }

        await db.SaveChangesAsync();

        // Recalc totals
        var servicesTotal = await db.BookingServices
            .Where(x => x.BookingId == bookingId)
            .SumAsync(x => x.LineTotal);

        booking.ServicesTotal = servicesTotal;
        booking.GrandTotal = booking.RoomTotal + booking.ServicesTotal;
        await db.SaveChangesAsync();
    }
}