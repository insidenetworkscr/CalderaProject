using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TallerCaldera.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public string Type { get; set; }

        public string Observations { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        public int? Mileage { get; set; }

        [Required]
        public string VehiclePlate { get; set; }

        [ForeignKey(nameof(VehiclePlate))]
        [ValidateNever]
        public Vehicle Vehicle { get; set; }

        [ValidateNever]
        public ICollection<MaintenancePhoto> Photos { get; set; } = new List<MaintenancePhoto>();

        [ValidateNever]
        public ICollection<SketchMark> SketchMarks { get; set; } = new List<SketchMark>();
    }
}
