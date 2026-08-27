using System.Windows;
using lab2.Datos;

namespace lab2.Vistas;

/// <summary>Fila del reporte de cargas (une el ingreso con su salida).</summary>
public class CargaReporte
{
    public string Numero { get; set; } = "";
    public DateTime Fecha { get; set; }
    public string Placa { get; set; } = "";
    public string Conductor { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string Producto { get; set; } = "";
    public string Transporte { get; set; } = "";
    public decimal PesoIngreso { get; set; }
    public decimal? PesoSalida { get; set; }
    public decimal? PesoNeto => PesoSalida.HasValue ? PesoSalida - PesoIngreso : null;
    public string Estado => PesoSalida.HasValue ? "Completado" : "Pendiente";
}

public partial class ReporteCargasWindow : Window
{
    public ReporteCargasWindow()
    {
        InitializeComponent();

        cboProducto.ItemsSource = Almacen.Productos.Select(p => p.Nombre).OrderBy(n => n).ToList();
        cboEstado.ItemsSource = new[] { "Todos", "Pendiente", "Completado" };
        cboEstado.SelectedIndex = 0;

        Buscar();
    }

    private static List<CargaReporte> ConstruirCargas()
        => Almacen.Ingresos.Select(i =>
        {
            var salida = Almacen.Salidas.FirstOrDefault(s => s.IngresoId == i.Id);
            return new CargaReporte
            {
                Numero = $"C-{i.Id:D4}",
                Fecha = i.FechaHora,
                Placa = i.Placa,
                Conductor = i.NombreConductor,
                Cliente = i.NombreCliente,
                Producto = i.Producto,
                Transporte = i.Transporte,
                PesoIngreso = i.PesoIngreso,
                PesoSalida = salida?.PesoSalida
            };
        }).ToList();

    private void Buscar()
    {
        IEnumerable<CargaReporte> consulta = ConstruirCargas();

        if (dtpInicio.SelectedDate is DateTime inicio)
            consulta = consulta.Where(c => c.Fecha.Date >= inicio.Date);

        if (dtpFin.SelectedDate is DateTime fin)
            consulta = consulta.Where(c => c.Fecha.Date <= fin.Date);

        var producto = cboProducto.Text.Trim();
        if (!string.IsNullOrWhiteSpace(producto))
            consulta = consulta.Where(c => c.Producto.Contains(producto, StringComparison.OrdinalIgnoreCase));

        var estado = cboEstado.SelectedItem?.ToString();
        if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
            consulta = consulta.Where(c => c.Estado == estado);

        var resultado = consulta.OrderByDescending(c => c.Fecha).ToList();
        dgCargas.ItemsSource = resultado;

        lblRegistros.Text = $"Cargas encontradas: {resultado.Count}";
        lblPesoTotal.Text = $"Peso neto total: {resultado.Sum(c => c.PesoNeto ?? 0):N2} Kg";
    }

    private void btnBuscar_Click(object sender, RoutedEventArgs e)
    {
        if (dtpInicio.SelectedDate is DateTime ini && dtpFin.SelectedDate is DateTime fin && ini > fin)
        {
            MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.", "Validacion",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Buscar();

        if (dgCargas.Items.Count == 0)
            MessageBox.Show("No se encontraron cargas con los filtros indicados.", "Busqueda",
                            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void btnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        dtpInicio.SelectedDate = null;
        dtpFin.SelectedDate = null;
        cboProducto.SelectedIndex = -1;
        cboProducto.Text = "";
        cboEstado.SelectedIndex = 0;
        Buscar();
    }

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();
}
