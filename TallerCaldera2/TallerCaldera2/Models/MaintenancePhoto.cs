using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TallerCaldera.Models
{
    public class MaintenancePhoto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int MaintenanceId { get; set; }

        [ForeignKey(nameof(MaintenanceId))]
        [ValidateNever]
        public Maintenance Maintenance { get; set; }
    }
}
