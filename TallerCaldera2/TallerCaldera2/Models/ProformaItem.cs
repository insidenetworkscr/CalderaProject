using System.ComponentModel.DataAnnotations.Schema;
using TallerCaldera2.Models;

public class ProformaItem
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    [NotMapped]
    public decimal Subtotal => Cantidad * PrecioUnitario;

    // FK
    public int ProformaId { get; set; }

    [NotMapped]
    public Proforma? Proforma { get; set; }
}
