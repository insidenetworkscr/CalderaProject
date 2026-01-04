using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera2.Models
{
    public class Proforma
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        public string ClienteNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string ClienteTelefono { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string ClienteEmail { get; set; } = string.Empty;

        public DateTime FechaEmision { get; set; }
        public DateTime FechaValidez { get; set; }

        public List<ProformaItem> Items { get; set; } = new();

        [NotMapped]
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
