using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera.Models
{
    public class FileAttachment
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string RelativePath { get; set; } // ruta en wwwroot/uploads/...
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Relacion opcional a Vehicle o Maintenance
        public int? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int? MaintenanceId { get; set; }
        public Maintenance Maintenance { get; set; }
    }
}
