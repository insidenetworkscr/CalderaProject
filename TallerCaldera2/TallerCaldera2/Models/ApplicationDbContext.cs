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
        public DbSet<Sketch> Sketches { get; set; }
        public DbSet<SketchMark> SketchMarks { get; set; }
        public DbSet<Alert> Alerts { get; set; }
   

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Vehicle>()
                .HasIndex(v => v.Plate)
                .IsUnique(false);

            b.Entity<Sketch>()
                .HasOne(s => s.Vehicle)
                .WithMany(v => v.Sketches)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<SketchMark>()
                .HasOne(m => m.Sketch)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.SketchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}