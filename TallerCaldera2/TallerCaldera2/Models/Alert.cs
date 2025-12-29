using System.ComponentModel.DataAnnotations;

namespace TallerCaldera2.Models


{
    public class Alert
    {
        public int Id { get; set; }

        // FK correcta
        [Required]
        public string VehiclePlate { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime DueDate { get; set; } // Próximo mantenimiento
        public string Message { get; set; }

        public bool IsShown { get; set; } = false;
    }
}
