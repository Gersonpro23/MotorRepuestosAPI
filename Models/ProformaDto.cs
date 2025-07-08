namespace MotorRepuestosAPI.Models.DTOs
{
    public class ProformaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }
        public List<DetalleProformaDto> Detalles { get; set; } = new();
    }

    public class DetalleProformaDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
