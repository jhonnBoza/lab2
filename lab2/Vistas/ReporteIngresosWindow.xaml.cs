using System.Windows;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class ReporteIngresosWindow : Window
{
    public ReporteIngresosWindow()
    {
        InitializeComponent();

        cboPlaca.ItemsSource = Almacen.Ingresos.Select(i => i.Placa).Distinct().OrderBy(p => p).ToList();
        cboConductor.ItemsSource = Almacen.Conductores.Select(c => c.Nombre).OrderBy(n => n).ToList();
        cboProducto.ItemsSource = Almacen.Productos.Select(p => p.Nombre).OrderBy(n => n).ToList();

        Buscar();
    }

    private void Buscar()
    {
        IEnumerable<Ingreso> consulta = Almacen.Ingresos;

        if (dtpInicio.SelectedDate is DateTime inicio)
            consulta = consulta.Where(i => i.FechaHora.Date >= inicio.Date);

        if (dtpFin.SelectedDate is DateTime fin)
            consulta = consulta.Where(i => i.FechaHora.Date <= fin.Date);

        var placa = cboPlaca.Text.Trim();
        if (!string.IsNullOrWhiteSpace(placa))
            consulta = consulta.Where(i => i.Placa.Contains(placa, StringComparison.OrdinalIgnoreCase));

        var conductor = cboConductor.Text.Trim();
        if (!string.IsNullOrWhiteSpace(conductor))
            consulta = consulta.Where(i => i.NombreConductor.Contains(conductor, StringComparison.OrdinalIgnoreCase));

        var producto = cboProducto.Text.Trim();
        if (!string.IsNullOrWhiteSpace(producto))
            consulta = consulta.Where(i => i.Producto.Contains(producto, StringComparison.OrdinalIgnoreCase));

        var resultado = consulta.OrderByDescending(i => i.FechaHora).ToList();
        dgIngresos.ItemsSource = resultado;

        lblRegistros.Text = $"Registros encontrados: {resultado.Count}";
        lblPesoTotal.Text = $"Peso total: {resultado.Sum(i => i.PesoIngreso):N2} Kg";
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

        if (dgIngresos.Items.Count == 0)
            MessageBox.Show("No se encontraron ingresos con los filtros indicados.", "Busqueda",
                            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void btnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        dtpInicio.SelectedDate = null;
        dtpFin.SelectedDate = null;
        cboPlaca.SelectedIndex = -1;
        cboPlaca.Text = "";
        cboConductor.SelectedIndex = -1;
        cboConductor.Text = "";
        cboProducto.SelectedIndex = -1;
        cboProducto.Text = "";
        Buscar();
    }

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();
}
