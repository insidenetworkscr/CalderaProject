using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TallerCaldera2.Models
{
    public class Vehicle
    {
        [Key]
        [Required]
        public string Plate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public string ClientName { get; set; }
        public string ClientIdNumber { get; set; }
        public string ClientPhone { get; set; }
        public string ClientAddress { get; set; }

        [EmailAddress]
        public string ClientEmail { get; set; }

        public string FuelType { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public string OilType { get; set; }

        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}
