using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string Type { get; set; }
        public string Observations { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        public int? Mileage { get; set; }

        [Required]
        public string VehiclePlate { get; set; }

        [ForeignKey(nameof(VehiclePlate))]
        public Vehicle Vehicle { get; set; }

        public ICollection<MaintenancePhoto> Photos { get; set; } = new List<MaintenancePhoto>();
    }
}
