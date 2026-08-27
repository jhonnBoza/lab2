using System.Collections.ObjectModel;
using lab2.Modelos;

namespace lab2.Datos;

/// <summary>
/// Almacen de datos en memoria de la aplicacion (simula la base de datos).
/// </summary>
public static class Almacen
{
    public static Usuario? UsuarioActual { get; set; }

    public static List<Usuario> Usuarios { get; } = new()
    {
        new Usuario { NombreUsuario = "admin",  Password = "123456", NombreCompleto = "Edwin Arevalo Sermeno" },
        new Usuario { NombreUsuario = "balanza", Password = "balanza", NombreCompleto = "Operador de Balanza" }
    };

    public static ObservableCollection<Transportista> Transportistas { get; } = new()
    {
        new Transportista { Id = 1, Nombre = "Transportes Andinos SAC", Ruc = "20501234567", Telefono = "987654321" },
        new Transportista { Id = 2, Nombre = "Logistica del Norte EIRL", Ruc = "20487654321", Telefono = "912345678" },
        new Transportista { Id = 3, Nombre = "Carga Segura SRL", Ruc = "20455566677", Telefono = "955443322" }
    };

    public static ObservableCollection<Conductor> Conductores { get; } = new()
    {
        new Conductor { Id = 1, Nombre = "Juan Perez Rojas",    Licencia = "Q12345678", Transporte = "Transportes Andinos SAC" },
        new Conductor { Id = 2, Nombre = "Carlos Diaz Mendoza", Licencia = "Q87654321", Transporte = "Logistica del Norte EIRL" },
        new Conductor { Id = 3, Nombre = "Miguel Torres Lopez", Licencia = "Q45612378", Transporte = "Carga Segura SRL" },
        new Conductor { Id = 4, Nombre = "Luis Ramos Quispe",   Licencia = "Q99988877", Transporte = "Transportes Andinos SAC" }
    };

    public static ObservableCollection<Camion> Camiones { get; } = new()
    {
        new Camion { Id = 1, Placa = "ABC-123", Marca = "Volvo",      Capacidad = 30000, Transporte = "Transportes Andinos SAC" },
        new Camion { Id = 2, Placa = "XYZ-789", Marca = "Scania",     Capacidad = 32000, Transporte = "Logistica del Norte EIRL" },
        new Camion { Id = 3, Placa = "JKL-456", Marca = "Mercedes",   Capacidad = 28000, Transporte = "Carga Segura SRL" },
        new Camion { Id = 4, Placa = "MNO-321", Marca = "Freightliner", Capacidad = 35000, Transporte = "Transportes Andinos SAC" }
    };

    public static ObservableCollection<Producto> Productos { get; } = new()
    {
        new Producto { Id = 1, Nombre = "Maiz Amarillo", Unidad = "KG" },
        new Producto { Id = 2, Nombre = "Trigo",         Unidad = "KG" },
        new Producto { Id = 3, Nombre = "Soya",          Unidad = "KG" },
        new Producto { Id = 4, Nombre = "Cemento",       Unidad = "KG" }
    };

    public static ObservableCollection<Ingreso> Ingresos { get; } = new()
    {
        new Ingreso { Id = 1, TipoDocumento = "Guia de Remision", NumeroDocumento = "001-000123", Placa = "ABC-123",
                      Turno = "Manana", NombreConductor = "Juan Perez Rojas", NombreCliente = "Molinos del Sur SAC",
                      FechaHora = DateTime.Today.AddDays(-3).AddHours(8),  PesoIngreso = 12500,
                      Producto = "Maiz Amarillo", Transporte = "Transportes Andinos SAC" },
        new Ingreso { Id = 2, TipoDocumento = "Factura", NumeroDocumento = "F001-004521", Placa = "XYZ-789",
                      Turno = "Tarde", NombreConductor = "Carlos Diaz Mendoza", NombreCliente = "Agroindustrias Peru SA",
                      FechaHora = DateTime.Today.AddDays(-2).AddHours(15), PesoIngreso = 15800,
                      Producto = "Trigo", Transporte = "Logistica del Norte EIRL" },
        new Ingreso { Id = 3, TipoDocumento = "Guia de Remision", NumeroDocumento = "001-000145", Placa = "JKL-456",
                      Turno = "Noche", NombreConductor = "Miguel Torres Lopez", NombreCliente = "Alimentos Andinos EIRL",
                      FechaHora = DateTime.Today.AddDays(-1).AddHours(21), PesoIngreso = 9700,
                      Producto = "Soya", Transporte = "Carga Segura SRL" },
        new Ingreso { Id = 4, TipoDocumento = "Ticket", NumeroDocumento = "T-000987", Placa = "MNO-321",
                      Turno = "Manana", NombreConductor = "Luis Ramos Quispe", NombreCliente = "Constructora Lima SAC",
                      FechaHora = DateTime.Today.AddHours(9), PesoIngreso = 18200,
                      Producto = "Cemento", Transporte = "Transportes Andinos SAC" }
    };

    public static ObservableCollection<Salida> Salidas { get; } = new()
    {
        new Salida { Id = 1, IngresoId = 1, Placa = "ABC-123", Turno = "Manana", NombreConductor = "Juan Perez Rojas",
                     NombreCliente = "Molinos del Sur SAC", Producto = "Maiz Amarillo", Transporte = "Transportes Andinos SAC",
                     FechaHora = DateTime.Today.AddDays(-3).AddHours(11), PesoIngreso = 12500, PesoSalida = 30400 },
        new Salida { Id = 2, IngresoId = 2, Placa = "XYZ-789", Turno = "Tarde", NombreConductor = "Carlos Diaz Mendoza",
                     NombreCliente = "Agroindustrias Peru SA", Producto = "Trigo", Transporte = "Logistica del Norte EIRL",
                     FechaHora = DateTime.Today.AddDays(-2).AddHours(18), PesoIngreso = 15800, PesoSalida = 31200 }
    };

    public static string[] TiposDocumento { get; } = { "Guia de Remision", "Factura", "Boleta", "Ticket" };
    public static string[] Turnos { get; } = { "Manana", "Tarde", "Noche" };

    public static int SiguienteId<T>(IEnumerable<T> lista, Func<T, int> selector)
        => lista.Any() ? lista.Max(selector) + 1 : 1;

    public static Usuario? Validar(string usuario, string password)
        => Usuarios.FirstOrDefault(u =>
               string.Equals(u.NombreUsuario, usuario, StringComparison.OrdinalIgnoreCase) &&
               u.Password == password);
}
