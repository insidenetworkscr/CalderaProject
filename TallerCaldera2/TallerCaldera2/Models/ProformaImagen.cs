namespace TallerCaldera2.Models
{
    public class ProformaImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int ProformaId { get; set; }
        public Proforma Proforma { get; set; } = null!;
    }
}

