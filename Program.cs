using Microsoft.EntityFrameworkCore;
using MotorRepuestosAPI.Data;
using MotorRepuestosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Servicios requeridos
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ejecutar creación del usuario admin (siempre, en cualquier entorno)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CrearUsuarioAdminAsync(app);
}

// Swagger solo si estás en desarrollo (opcional)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();


// ========================
// 👤 Lógica para crear ADMIN
// ========================
async Task CrearUsuarioAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var emailAdmin = "admin@admin.com";
    var yaExiste = await db.Usuarios.AnyAsync(u => u.Email == emailAdmin);

    if (!yaExiste)
    {
        CrearPasswordHash("admin123", out byte[] hash, out byte[] salt);

        var admin = new Usuario
        {
            NombreUsuario = "Administrador",
            Email = emailAdmin,
            PasswordHash = hash,
            PasswordSalt = salt,
            Telefono = "999999999",
            Rol = "Admin"
        };

        db.Usuarios.Add(admin);
        await db.SaveChangesAsync();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✔ Usuario administrador creado correctamente.");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("ℹ Usuario admin ya existe. No se creó uno nuevo.");
        Console.ResetColor();
    }
}

// ========================
// 🔐 Método para generar password hash
// ========================
void CrearPasswordHash(string password, out byte[] hash, out byte[] salt)
{
    using var hmac = new System.Security.Cryptography.HMACSHA512();
    salt = hmac.Key;
    hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
}

