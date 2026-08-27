namespace lab2.Modelos;

public class Usuario
{
    public string NombreUsuario { get; set; } = "";
    public string Password { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
}

public class Transportista
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Ruc { get; set; } = "";
    public string Telefono { get; set; } = "";
    public override string ToString() => Nombre;
}

public class Conductor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Licencia { get; set; } = "";
    public string Transporte { get; set; } = "";
    public override string ToString() => Nombre;
}

public class Camion
{
    public int Id { get; set; }
    public string Placa { get; set; } = "";
    public string Marca { get; set; } = "";
    public decimal Capacidad { get; set; }
    public string Transporte { get; set; } = "";
    public override string ToString() => Placa;
}

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Unidad { get; set; } = "";
    public override string ToString() => Nombre;
}

public class Ingreso
{
    public int Id { get; set; }
    public string TipoDocumento { get; set; } = "";
    public string NumeroDocumento { get; set; } = "";
    public string Placa { get; set; } = "";
    public string Turno { get; set; } = "";
    public string NombreConductor { get; set; } = "";
    public string NombreCliente { get; set; } = "";
    public DateTime FechaHora { get; set; }
    public decimal PesoIngreso { get; set; }
    public string Producto { get; set; } = "";
    public string Transporte { get; set; } = "";

    public string Descripcion => $"{Id:D4} - {Placa} - {NombreConductor} ({FechaHora:dd/MM/yyyy HH:mm})";
}

public class Salida
{
    public int Id { get; set; }
    public int IngresoId { get; set; }
    public string Placa { get; set; } = "";
    public string Turno { get; set; } = "";
    public string NombreConductor { get; set; } = "";
    public string NombreCliente { get; set; } = "";
    public string Producto { get; set; } = "";
    public string Transporte { get; set; } = "";
    public DateTime FechaHora { get; set; }
    public decimal PesoIngreso { get; set; }
    public decimal PesoSalida { get; set; }
    public decimal PesoNeto => PesoSalida - PesoIngreso;
}
