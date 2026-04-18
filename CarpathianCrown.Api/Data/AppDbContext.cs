using CarpathianCrown.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();
    public DbSet<BookingService> BookingServices => Set<BookingService>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ContentPage> ContentPages => Set<ContentPage>();
    public DbSet<RoomOrder> RoomOrders => Set<RoomOrder>();
    public DbSet<RoomOrderItem> RoomOrderItems => Set<RoomOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Login).IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Review)
            .WithOne(r => r.Booking)
            .HasForeignKey<Review>(r => r.BookingId);

        modelBuilder.Entity<RoomOrder>()
            .HasMany(x => x.Items)
            .WithOne(x => x.RoomOrder)
            .HasForeignKey(x => x.RoomOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomOrder>()
            .HasOne(x => x.Booking)
            .WithMany()
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RoomOrder>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RoomOrderItem>()
            .HasOne(x => x.ServiceItem)
            .WithMany()
            .HasForeignKey(x => x.ServiceItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}