using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class SalidaWindow : Window
{
    public SalidaWindow()
    {
        InitializeComponent();
        dgSalidas.ItemsSource = Almacen.Salidas;
        Limpiar();
    }

    private void CargarPendientes()
    {
        var conSalida = Almacen.Salidas.Select(s => s.IngresoId).ToHashSet();
        cboIngreso.ItemsSource = Almacen.Ingresos.Where(i => !conSalida.Contains(i.Id)).ToList();
        cboIngreso.SelectedIndex = -1;
    }

    private void Limpiar()
    {
        CargarPendientes();
        txtPlaca.Clear();
        txtConductor.Clear();
        txtProducto.Clear();
        txtPesoIngreso.Clear();
        txtPesoSalida.Clear();
        dtpFecha.SelectedDate = DateTime.Today;
        txtHora.Text = DateTime.Now.ToString("HH:mm");
        lblNeto.Text = "Peso neto: 0.00 Kg";
        cboIngreso.Focus();
    }

    private void cboIngreso_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cboIngreso.SelectedItem is not Ingreso i) return;
        txtPlaca.Text = i.Placa;
        txtConductor.Text = i.NombreConductor;
        txtProducto.Text = i.Producto;
        txtPesoIngreso.Text = i.PesoIngreso.ToString("N2");
        CalcularNeto();
    }

    private void txtPesoSalida_TextChanged(object sender, TextChangedEventArgs e) => CalcularNeto();

    private void CalcularNeto()
    {
        if (lblNeto is null) return;
        var ingreso = cboIngreso.SelectedItem as Ingreso;
        decimal.TryParse(txtPesoSalida.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var salida);
        var neto = salida - (ingreso?.PesoIngreso ?? 0);
        lblNeto.Text = $"Peso neto: {neto:N2} Kg";
    }

    private void SoloNumeros(object sender, TextCompositionEventArgs e)
        => e.Handled = !Regex.IsMatch(e.Text, @"^[0-9.]$");

    private void btnNuevo_Click(object sender, RoutedEventArgs e) => Limpiar();

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        if (cboIngreso.SelectedItem is not Ingreso ingreso)
        {
            Aviso("Seleccione el ingreso al que corresponde la salida.");
            cboIngreso.Focus();
            return;
        }

        if (dtpFecha.SelectedDate is null)
        {
            Aviso("Seleccione la fecha de salida.");
            dtpFecha.Focus();
            return;
        }

        if (!TimeSpan.TryParseExact(txtHora.Text.Trim(), @"hh\:mm", CultureInfo.InvariantCulture, out var hora))
        {
            Aviso("Ingrese una hora valida con el formato HH:mm.");
            txtHora.Focus();
            return;
        }

        if (!decimal.TryParse(txtPesoSalida.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var pesoSalida)
            || pesoSalida <= 0)
        {
            Aviso("Ingrese un peso de salida valido mayor a cero.");
            txtPesoSalida.Focus();
            return;
        }

        if (pesoSalida <= ingreso.PesoIngreso)
        {
            Aviso("El peso de salida debe ser mayor al peso de ingreso.");
            txtPesoSalida.Focus();
            return;
        }

        var fechaSalida = dtpFecha.SelectedDate.Value.Add(hora);
        if (fechaSalida < ingreso.FechaHora)
        {
            Aviso("La fecha y hora de salida no puede ser anterior a la del ingreso.");
            dtpFecha.Focus();
            return;
        }

        Almacen.Salidas.Add(new Salida
        {
            Id = Almacen.SiguienteId(Almacen.Salidas, s => s.Id),
            IngresoId = ingreso.Id,
            Placa = ingreso.Placa,
            Turno = ingreso.Turno,
            NombreConductor = ingreso.NombreConductor,
            NombreCliente = ingreso.NombreCliente,
            Producto = ingreso.Producto,
            Transporte = ingreso.Transporte,
            FechaHora = fechaSalida,
            PesoIngreso = ingreso.PesoIngreso,
            PesoSalida = pesoSalida
        });

        MessageBox.Show("Salida registrada correctamente.", "Registro de Salida",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        Limpiar();
    }

    private static void Aviso(string mensaje)
        => MessageBox.Show(mensaje, "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
}
