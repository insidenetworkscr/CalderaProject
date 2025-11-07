using System.ComponentModel.DataAnnotations;
using TallerCaldera2.Models;

namespace TallerCaldera.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Plate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public string ClientName { get; set; }
        public string ClientIdNumber { get; set; }
        public string ClientPhone { get; set; }

        public string FuelType { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public string OilType { get; set; }

        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
        public ICollection<Sketch> Sketches { get; set; } = new List<Sketch>();
    }
}