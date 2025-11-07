using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TallerCaldera.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public string Plate { get; set; } // para búsquedas rápidas (redundancia)

        public string Type { get; set; } // Tipo de mantenimiento
        public string Observations { get; set; }
        public decimal? Cost { get; set; }
        public int? Mileage { get; set; } // Kilometraje



        // FK a vehicle
        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

    }
}

