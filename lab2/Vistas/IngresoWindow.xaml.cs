using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class IngresoWindow : Window
{
    public IngresoWindow()
    {
        InitializeComponent();

        cboTipoDocumento.ItemsSource = Almacen.TiposDocumento;
        cboTurno.ItemsSource = Almacen.Turnos;
        cboPlaca.ItemsSource = Almacen.Camiones.Select(c => c.Placa).ToList();
        cboConductor.ItemsSource = Almacen.Conductores.ToList();
        cboConductor.DisplayMemberPath = nameof(Conductor.Nombre);
        cboProducto.ItemsSource = Almacen.Productos.Select(p => p.Nombre).ToList();

        dgIngresos.ItemsSource = Almacen.Ingresos;
        Limpiar();
    }

    private void Limpiar()
    {
        cboTipoDocumento.SelectedIndex = 0;
        txtNumeroDocumento.Clear();
        cboPlaca.SelectedIndex = -1;
        cboPlaca.Text = "";
        cboTurno.SelectedIndex = TurnoSugerido();
        cboConductor.SelectedIndex = -1;
        cboConductor.Text = "";
        txtCliente.Clear();
        dtpFecha.SelectedDate = DateTime.Today;
        txtHora.Text = DateTime.Now.ToString("HH:mm");
        txtPeso.Clear();
        cboProducto.SelectedIndex = -1;
        txtTransporte.Clear();
        cboTipoDocumento.Focus();
    }

    private static int TurnoSugerido()
    {
        var h = DateTime.Now.Hour;
        if (h < 13) return 0;          // Manana
        return h < 19 ? 1 : 2;         // Tarde / Noche
    }

    private void cboConductor_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cboConductor.SelectedItem is Conductor c)
            txtTransporte.Text = c.Transporte;
    }

    private void SoloNumeros(object sender, TextCompositionEventArgs e)
        => e.Handled = !Regex.IsMatch(e.Text, @"^[0-9.]$");

    private void btnNuevo_Click(object sender, RoutedEventArgs e) => Limpiar();

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        if (!Validar(out var ingreso)) return;

        Almacen.Ingresos.Add(ingreso!);
        MessageBox.Show($"Ingreso registrado correctamente con el numero {ingreso!.Id:D4}.",
                        "Registro de Ingresos", MessageBoxButton.OK, MessageBoxImage.Information);
        Limpiar();
    }

    private bool Validar(out Ingreso? ingreso)
    {
        ingreso = null;

        if (cboTipoDocumento.SelectedItem is null)
            return Error("Seleccione el tipo de documento.", cboTipoDocumento);

        if (string.IsNullOrWhiteSpace(txtNumeroDocumento.Text))
            return Error("Ingrese el numero de documento.", txtNumeroDocumento);

        var placa = cboPlaca.Text.Trim();
        if (string.IsNullOrWhiteSpace(placa))
            return Error("Ingrese o seleccione la placa del vehiculo.", cboPlaca);

        if (cboTurno.SelectedItem is null)
            return Error("Seleccione el turno.", cboTurno);

        var conductor = cboConductor.SelectedItem as Conductor;
        var nombreConductor = conductor?.Nombre ?? cboConductor.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombreConductor))
            return Error("Ingrese o seleccione el nombre del conductor.", cboConductor);

        if (string.IsNullOrWhiteSpace(txtCliente.Text))
            return Error("Ingrese el nombre del cliente.", txtCliente);

        if (dtpFecha.SelectedDate is null)
            return Error("Seleccione la fecha de ingreso.", dtpFecha);

        if (!TimeSpan.TryParseExact(txtHora.Text.Trim(), @"hh\:mm", CultureInfo.InvariantCulture, out var hora))
            return Error("Ingrese una hora valida con el formato HH:mm (ejemplo 14:30).", txtHora);

        if (!decimal.TryParse(txtPeso.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var peso) || peso <= 0)
            return Error("Ingrese un peso de ingreso valido mayor a cero.", txtPeso);

        ingreso = new Ingreso
        {
            Id = Almacen.SiguienteId(Almacen.Ingresos, i => i.Id),
            TipoDocumento = cboTipoDocumento.SelectedItem!.ToString()!,
            NumeroDocumento = txtNumeroDocumento.Text.Trim(),
            Placa = placa.ToUpperInvariant(),
            Turno = cboTurno.SelectedItem!.ToString()!,
            NombreConductor = nombreConductor,
            NombreCliente = txtCliente.Text.Trim(),
            FechaHora = dtpFecha.SelectedDate!.Value.Add(hora),
            PesoIngreso = peso,
            Producto = cboProducto.SelectedItem?.ToString() ?? "",
            Transporte = string.IsNullOrWhiteSpace(txtTransporte.Text)
                            ? (conductor?.Transporte ?? "")
                            : txtTransporte.Text.Trim()
        };
        return true;
    }

    private static bool Error(string mensaje, Control control)
    {
        MessageBox.Show(mensaje, "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
        control.Focus();
        return false;
    }
}
