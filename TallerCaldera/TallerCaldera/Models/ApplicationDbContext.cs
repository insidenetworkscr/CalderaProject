using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TallerCaldera.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<FileAttachment> FileAttachments { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
