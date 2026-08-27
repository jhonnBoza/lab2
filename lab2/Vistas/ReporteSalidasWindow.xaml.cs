using System.Windows;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class ReporteSalidasWindow : Window
{
    public ReporteSalidasWindow()
    {
        InitializeComponent();

        cboPlaca.ItemsSource = Almacen.Salidas.Select(s => s.Placa).Distinct().OrderBy(p => p).ToList();
        cboConductor.ItemsSource = Almacen.Conductores.Select(c => c.Nombre).OrderBy(n => n).ToList();

        Buscar();
    }

    private void Buscar()
    {
        IEnumerable<Salida> consulta = Almacen.Salidas;

        if (dtpInicio.SelectedDate is DateTime inicio)
            consulta = consulta.Where(s => s.FechaHora.Date >= inicio.Date);

        if (dtpFin.SelectedDate is DateTime fin)
            consulta = consulta.Where(s => s.FechaHora.Date <= fin.Date);

        var placa = cboPlaca.Text.Trim();
        if (!string.IsNullOrWhiteSpace(placa))
            consulta = consulta.Where(s => s.Placa.Contains(placa, StringComparison.OrdinalIgnoreCase));

        var conductor = cboConductor.Text.Trim();
        if (!string.IsNullOrWhiteSpace(conductor))
            consulta = consulta.Where(s => s.NombreConductor.Contains(conductor, StringComparison.OrdinalIgnoreCase));

        var resultado = consulta.OrderByDescending(s => s.FechaHora).ToList();
        dgSalidas.ItemsSource = resultado;

        lblRegistros.Text = $"Registros encontrados: {resultado.Count}";
        lblPesoTotal.Text = $"Peso neto total: {resultado.Sum(s => s.PesoNeto):N2} Kg";
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

        if (dgSalidas.Items.Count == 0)
            MessageBox.Show("No se encontraron salidas con los filtros indicados.", "Busqueda",
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
        Buscar();
    }

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();
}
