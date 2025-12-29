using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TallerCaldera2.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del cliente requerido")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Teléfono requerido")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Carro requerido")]
        public string Vehicle { get; set; }

        [Required(ErrorMessage = "Placa requerida")]
        public string Plate { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pendiente;
    }

    public enum AppointmentStatus
    {
        Pendiente,
        Confirmada
    }
}