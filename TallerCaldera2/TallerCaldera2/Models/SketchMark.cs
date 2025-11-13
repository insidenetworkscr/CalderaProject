using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera.Models
{
    public class SketchMark
    {
        [Key]
        public int Id { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public int MaintenanceId { get; set; }

        [ForeignKey(nameof(MaintenanceId))]
        [ValidateNever]
        public Maintenance Maintenance { get; set; }
    }
}
