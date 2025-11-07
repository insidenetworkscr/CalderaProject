using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera.Models
{
    // Boceto (imagen base con sus marcas)
    public class Sketch
    {
        [Key] public int Id { get; set; }
        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))] public Vehicle Vehicle { get; set; }

        // opcional: para consultas rápidas
        public string Plate { get; set; }

        // imagen base del boceto (png/jpg del carro “vacío”)
        public string BaseImagePath { get; set; } // ej: "/uploads/sketches/base-sedan.png"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SketchMark> Marks { get; set; } = new List<SketchMark>();
    }

    // Marca puntual sobre el boceto
    public class SketchMark
    {
        [Key] public int Id { get; set; }
        public int SketchId { get; set; }
        [ForeignKey(nameof(SketchId))] public Sketch Sketch { get; set; }

        // Coordenadas relativas 0..1 (para que escale bien con el tamaño del img)
        public double X { get; set; }
        public double Y { get; set; }

        public string Label { get; set; }      // ej. “Golpe”, “Rasguño”
        public string Severity { get; set; }   // ej. “low|med|high”
        public string Notes { get; set; }      // texto adicional
    }
}
