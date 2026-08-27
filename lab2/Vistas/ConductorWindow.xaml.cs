using System.Windows;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class ConductorWindow : Window
{
    public ConductorWindow()
    {
        InitializeComponent();
        cboTransporte.ItemsSource = Almacen.Transportistas.Select(t => t.Nombre).ToList();
        dgConductores.ItemsSource = Almacen.Conductores;
        Limpiar();
    }

    private void Limpiar()
    {
        txtNombre.Clear();
        txtLicencia.Clear();
        cboTransporte.SelectedIndex = -1;
        cboTransporte.Text = "";
        txtNombre.Focus();
    }

    private void btnNuevo_Click(object sender, RoutedEventArgs e) => Limpiar();

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

    private void btnLista_Click(object sender, RoutedEventArgs e)
        => new ConductorListaWindow { Owner = this }.ShowDialog();

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            Aviso("Ingrese el nombre del conductor.");
            txtNombre.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtLicencia.Text))
        {
            Aviso("Ingrese el numero de licencia.");
            txtLicencia.Focus();
            return;
        }

        var licencia = txtLicencia.Text.Trim();
        if (Almacen.Conductores.Any(c => string.Equals(c.Licencia, licencia, StringComparison.OrdinalIgnoreCase)))
        {
            Aviso($"Ya existe un conductor registrado con la licencia {licencia}.");
            txtLicencia.Focus();
            return;
        }

        var transporte = cboTransporte.Text.Trim();
        if (string.IsNullOrWhiteSpace(transporte))
        {
            Aviso("Seleccione o ingrese la empresa de transporte.");
            cboTransporte.Focus();
            return;
        }

        Almacen.Conductores.Add(new Conductor
        {
            Id = Almacen.SiguienteId(Almacen.Conductores, c => c.Id),
            Nombre = txtNombre.Text.Trim(),
            Licencia = licencia,
            Transporte = transporte
        });

        MessageBox.Show("Conductor registrado correctamente.", "Mantenimiento de Conductores",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        Limpiar();
    }

    private static void Aviso(string mensaje)
        => MessageBox.Show(mensaje, "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
}
