using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera2.Models
{
    public class MaintenanceItem
    {
        public int Id { get; set; }

        [Required]
        public string Servicio { get; set; } = string.Empty;

        public int Unidad { get; set; }

        public decimal Precio { get; set; }

        // FK
        public int MaintenanceId { get; set; }

        public Maintenance? Maintenance { get; set; }

        [NotMapped]
        public decimal Subtotal => Unidad * Precio;
    }
}
