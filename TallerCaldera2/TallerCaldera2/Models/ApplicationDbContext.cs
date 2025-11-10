using System.Collections.Generic;
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

            b.Entity<Vehicle>()
               .HasKey(v => v.Plate);

            b.Entity<Vehicle>()
                .HasIndex(v => v.Plate)
                .IsUnique(true);

            //  Maintenance
            b.Entity<Maintenance>()
                .HasOne(m => m.Vehicle)
                .WithMany(v => v.Maintenances)
                .HasForeignKey(m => m.VehiclePlate)
                .HasPrincipalKey(v => v.Plate)
                .OnDelete(DeleteBehavior.Cascade);

            /*  Sketch
            b.Entity<Sketch>()
                .HasOne(s => s.Vehicle)
                .WithMany(v => v.Sketches)
                .HasForeignKey(s => s.VehiclePlate)
                .HasPrincipalKey(v => v.Plate)
                .OnDelete(DeleteBehavior.Cascade);

            //  SketchMark
            b.Entity<SketchMark>()
                .HasOne(m => m.Sketch)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.SketchId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}