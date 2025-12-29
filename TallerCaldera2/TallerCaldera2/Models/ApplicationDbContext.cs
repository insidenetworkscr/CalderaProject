using Microsoft.EntityFrameworkCore;
using TallerCaldera2.Models;

namespace TallerCaldera2.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<MaintenancePhoto> MaintenancePhotos { get; set; }
        public DbSet<SketchMark> SketchMarks { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // VEHICLE
            b.Entity<Vehicle>()
                .HasKey(v => v.Plate);

            b.Entity<Vehicle>()
                .HasIndex(v => v.Plate)
                .IsUnique(true);

            // MAINTENANCE
            b.Entity<Maintenance>()
                .HasOne(m => m.Vehicle)
                .WithMany(v => v.Maintenances)
                .HasForeignKey(m => m.VehiclePlate)
                .HasPrincipalKey(v => v.Plate)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Maintenance>()
                .Property(m => m.Cost)
                .HasColumnType("decimal(18,2)");

            // PHOTOS
            b.Entity<MaintenancePhoto>()
                .HasOne(p => p.Maintenance)
                .WithMany(m => m.Photos)
                .HasForeignKey(p => p.MaintenanceId)
                .OnDelete(DeleteBehavior.Cascade);

            // SKETCH MARKS
            b.Entity<SketchMark>()
                .HasOne(s => s.Maintenance)
                .WithMany(m => m.SketchMarks)
                .HasForeignKey(s => s.MaintenanceId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Alert>()
                .HasOne(a => a.Vehicle)
                .WithMany()
                .HasForeignKey(a => a.VehiclePlate)
                .HasPrincipalKey(v => v.Plate)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Alert>()
                .Property(a => a.Message)
                .HasMaxLength(500);

            b.Entity<Alert>()
                .HasOne(a => a.Vehicle)
                .WithMany()
                .HasForeignKey(a => a.VehiclePlate)
                .HasPrincipalKey(v => v.Plate)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
