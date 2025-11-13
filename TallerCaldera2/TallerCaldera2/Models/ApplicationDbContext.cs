using Microsoft.EntityFrameworkCore;
using TallerCaldera.Models;

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
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // VEHICLE
            b.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Plate);

                entity.HasIndex(v => v.Plate)
                      .IsUnique(true);
            });

            // MAINTENANCE
            b.Entity<Maintenance>(entity =>
            {
                entity.Property(m => m.Cost)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(m => m.Vehicle)
                      .WithMany(v => v.Maintenances)
                      .HasForeignKey(m => m.VehiclePlate)
                      .HasPrincipalKey(v => v.Plate)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
