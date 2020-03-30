using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarRental.Models.Entities
{
    public partial class CarRentalContext : DbContext
    {
        public CarRentalContext()
        {
        }

        public CarRentalContext(DbContextOptions<CarRentalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingNumber)
                    .HasName("PK__Booking__AAC320BE51961604");

                entity.Property(e => e.CarLicenseNumber)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CarType)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ClientSsn)
                    .HasColumnName("ClientSSN")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.TimeOfBooking).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
