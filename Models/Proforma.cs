namespace MotorRepuestosAPI.Models
{
    public class Proforma
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public Usuario Usuario { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public ICollection<DetalleProforma> Detalles { get; set; }
    }
}