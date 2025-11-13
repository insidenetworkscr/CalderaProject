using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TallerCaldera.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(200)]
        public string Type { get; set; } // Tipo de mantenimiento

        [StringLength(2000)]
        public string? Observations { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        public int? Mileage { get; set; } // Kilometraje

        // 🔹 Llave foránea basada en la placa
        [Required]
        public string VehiclePlate { get; set; }

        // 🔹 Propiedad de navegación (NO se valida en el ModelState)
        [ForeignKey(nameof(VehiclePlate))]
        [ValidateNever]
        public Vehicle? Vehicle { get; set; }
    }
}
