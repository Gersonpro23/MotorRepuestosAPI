namespace MotorRepuestosAPI.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Codigo { get; set; }
        public int Stock { get; set; } = 0;
        public int CategoriaMotorId { get; set; }
        public CategoriaMotor CategoriaMotor { get; set; }
    }
}