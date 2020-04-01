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
        public virtual DbSet<ReturnOfRentalCar> ReturnOfRentalCar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarRental;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

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
                    .HasName("PK__Booking__AAC320BECD8AFF54");

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
            });

            modelBuilder.Entity<ReturnOfRentalCar>(entity =>
            {
                entity.Property(e => e.TimeOfReturn).HasColumnType("datetime");

                entity.HasOne(d => d.BookingNumberNavigation)
                    .WithMany(p => p.ReturnOfRentalCar)
                    .HasForeignKey(d => d.BookingNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReturnOfR__Booki__787EE5A0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
