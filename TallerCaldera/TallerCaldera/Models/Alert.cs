namespace TallerCaldera.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime DueDate { get; set; } // Fecha estimada/ próxima
        public string Message { get; set; }
        public bool IsShown { get; set; } = false;
    }
}
