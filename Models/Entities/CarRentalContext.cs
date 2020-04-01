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

        public virtual DbSet<AvailableCars> AvailableCars { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<ReturnOfRentalCar> ReturnOfRentalCar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvailableCars>(entity =>
            {
                entity.HasIndex(e => e.CarLicenseNumber)
                    .HasName("UQ__Availabl__BAF49BE59DD05331")
                    .IsUnique();

                entity.Property(e => e.CarLicenseNumber)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CarType)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.IsAvailable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingNumber)
                    .HasName("PK__Booking__AAC320BE34218BFD");

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

                entity.Property(e => e.TimeOfBooking).HasColumnType("datetime");

                entity.HasOne(d => d.ClientSsnNavigation)
                    .WithMany(p => p.Booking)
                    .HasPrincipalKey(p => p.ClientSsn)
                    .HasForeignKey(d => d.ClientSsn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__ClientS__09A971A2");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasIndex(e => e.ClientSsn)
                    .HasName("UQ__Clients__775D2E9C99FC423E")
                    .IsUnique();

                entity.Property(e => e.ClientSsn)
                    .IsRequired()
                    .HasColumnName("ClientSSN")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReturnOfRentalCar>(entity =>
            {
                entity.Property(e => e.TimeOfReturn).HasColumnType("datetime");

                entity.HasOne(d => d.BookingNumberNavigation)
                    .WithMany(p => p.ReturnOfRentalCar)
                    .HasForeignKey(d => d.BookingNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReturnOfR__Booki__0D7A0286");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
