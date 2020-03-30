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
        public virtual DbSet<ReturnOfRentalCar> ReturnOfRentalCar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingNumber)
                    .HasName("PK__Booking__AAC320BE9BB3046A");

                entity.Property(e => e.CarLicenseNumber)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CarType)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ClientSsn)
                    .IsRequired()
                    .HasColumnName("ClientSSN")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.TimeOfBooking).HasColumnType("date");
            });

            modelBuilder.Entity<ReturnOfRentalCar>(entity =>
            {
                entity.Property(e => e.TimeOfReturn).HasColumnType("date");

                entity.HasOne(d => d.BookingNumberNavigation)
                    .WithMany(p => p.ReturnOfRentalCar)
                    .HasForeignKey(d => d.BookingNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReturnOfR__Booki__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
