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
        public DbSet<FileAttachment> FileAttachments { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        
        // public DbSet<Usuario> Usuarios { get; set; }
    }
}