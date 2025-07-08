using Microsoft.EntityFrameworkCore;
using MotorRepuestosAPI.Models;

namespace MotorRepuestosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<CategoriaMotor> CategoriasMotores { get; set; }
        public DbSet<Proforma> Proformas { get; set; }
        public DbSet<DetalleProforma> DetallesProforma { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de precisión para decimales
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2); // 18 dígitos totales, 2 decimales

            modelBuilder.Entity<DetalleProforma>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Proforma>()
                .Property(p => p.Total)
                .HasPrecision(18, 2);

            // Configuraciones adicionales (índices y relaciones)
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Codigo)
                .IsUnique();

            modelBuilder.Entity<Proforma>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Proforma)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}