namespace MotorRepuestosAPI.Models
{
    public class DetalleProforma
    {
        public int Id { get; set; }
        public int ProformaId { get; set; }
        public Proforma Proforma { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total => Cantidad * PrecioUnitario;
    }
}