using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class CamionWindow : Window
{
    public CamionWindow()
    {
        InitializeComponent();
        cboTransporte.ItemsSource = Almacen.Transportistas.Select(t => t.Nombre).ToList();
        dgCamiones.ItemsSource = Almacen.Camiones;
        Limpiar();
    }

    private void Limpiar()
    {
        txtPlaca.Clear();
        txtMarca.Clear();
        txtCapacidad.Clear();
        cboTransporte.SelectedIndex = -1;
        cboTransporte.Text = "";
        txtPlaca.Focus();
    }

    private void SoloNumeros(object sender, TextCompositionEventArgs e)
        => e.Handled = !Regex.IsMatch(e.Text, @"^[0-9.]$");

    private void btnNuevo_Click(object sender, RoutedEventArgs e) => Limpiar();

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        var placa = txtPlaca.Text.Trim();
        if (string.IsNullOrWhiteSpace(placa))
        {
            Aviso("Ingrese la placa del camion.");
            txtPlaca.Focus();
            return;
        }

        if (Almacen.Camiones.Any(c => string.Equals(c.Placa, placa, StringComparison.OrdinalIgnoreCase)))
        {
            Aviso($"Ya existe un camion registrado con la placa {placa}.");
            txtPlaca.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtMarca.Text))
        {
            Aviso("Ingrese la marca del camion.");
            txtMarca.Focus();
            return;
        }

        if (!decimal.TryParse(txtCapacidad.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var capacidad)
            || capacidad <= 0)
        {
            Aviso("Ingrese una capacidad valida mayor a cero.");
            txtCapacidad.Focus();
            return;
        }

        var transporte = cboTransporte.Text.Trim();
        if (string.IsNullOrWhiteSpace(transporte))
        {
            Aviso("Seleccione o ingrese la empresa de transporte.");
            cboTransporte.Focus();
            return;
        }

        Almacen.Camiones.Add(new Camion
        {
            Id = Almacen.SiguienteId(Almacen.Camiones, c => c.Id),
            Placa = placa,
            Marca = txtMarca.Text.Trim(),
            Capacidad = capacidad,
            Transporte = transporte
        });

        MessageBox.Show("Camion registrado correctamente.", "Mantenimiento de Camiones",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        Limpiar();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (dgCamiones.SelectedItem is not Camion c)
        {
            Aviso("Seleccione un camion de la lista.");
            return;
        }

        if (MessageBox.Show($"Desea eliminar el camion de placa {c.Placa}?", "Confirmar",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

        Almacen.Camiones.Remove(c);
    }

    private static void Aviso(string mensaje)
        => MessageBox.Show(mensaje, "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
}
