using Microsoft.EntityFrameworkCore;
using MyTherapy.Domain.Entities;

namespace MyTherapy.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<User> Users => Set<User>(); // Add DbSet properties for other entities as needed
    public DbSet<Therapist> Therapists => Set<Therapist>(); // Example DbSet for a Therapist entity
    public DbSet<AvailabilitySlot> AvailabilitySlots => Set<AvailabilitySlot>(); // Example DbSet for AvailabilitySlot entity
    public DbSet<Booking> Bookings => Set<Booking>(); // Example DbSet for Booking entity

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Patient)
            .WithMany()
            .HasForeignKey(b => b.PatientId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Therapist)
            .WithMany()
            .HasForeignKey(b => b.TherapistId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Slot)
            .WithMany()
            .HasForeignKey(b => b.SlotId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
