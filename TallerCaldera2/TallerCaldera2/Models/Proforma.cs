using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerCaldera2.Models
{
    public class Proforma
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        [Required]
        public string ClienteNombre { get; set; } = string.Empty;

        public string ClienteTelefono { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string ClienteEmail { get; set; } = string.Empty;

        public DateTime FechaEmision { get; set; }
        public DateTime FechaValidez { get; set; }

        // ✅ NO MAPEADAS A BD
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;

        public bool AplicarIva { get; set; }

        public List<ProformaItem> Items { get; set; } = new();

        // CÁLCULOS
        [NotMapped]
        public decimal Subtotal => Items.Sum(i => i.Subtotal);

        [NotMapped]
        public decimal Iva => AplicarIva ? Subtotal * 0.13m : 0;

        [NotMapped]
        public decimal TotalConIva => Subtotal + Iva;

        public string? ImagenUrl { get; set; }

        public List<ProformaImage> Images { get; set; } = new();
    }
}