using System.ComponentModel.DataAnnotations;

namespace TallerCaldera.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Plate { get; set; } // Placa

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Brand { get; set; } // Marca
        public string Model { get; set; } // Modelo
        public int Year { get; set; } //Año
        public string ClientName { get; set; } //Nombre
        public string ClientIdNumber { get; set; } // Cedula
        public string ClientPhone { get; set; } //Telefono
        public string FuelType { get; set; } // Tipo de gasolina
        public DateTime? LastMaintenanceDate { get; set; } //Ultimo mantenimiento
        public string OilType { get; set; } //Tipo de aceite

     
        public string SketchMarksJson { get; set; }

        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
        public ICollection<FileAttachment> Attachments { get; set; } = new List<FileAttachment>();
    }
}
