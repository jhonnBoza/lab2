using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class TransportistaWindow : Window
{
    public TransportistaWindow()
    {
        InitializeComponent();
        dgTransportistas.ItemsSource = Almacen.Transportistas;
        Limpiar();
    }

    private void Limpiar()
    {
        txtNombre.Clear();
        txtRuc.Clear();
        txtTelefono.Clear();
        txtNombre.Focus();
    }

    private void SoloDigitos(object sender, TextCompositionEventArgs e)
        => e.Handled = !Regex.IsMatch(e.Text, "^[0-9]$");

    private void btnNuevo_Click(object sender, RoutedEventArgs e) => Limpiar();

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            Aviso("Ingrese el nombre o razon social del transportista.");
            txtNombre.Focus();
            return;
        }

        var ruc = txtRuc.Text.Trim();
        if (ruc.Length != 11)
        {
            Aviso("El RUC debe tener 11 digitos.");
            txtRuc.Focus();
            return;
        }

        if (Almacen.Transportistas.Any(t => t.Ruc == ruc))
        {
            Aviso($"Ya existe un transportista registrado con el RUC {ruc}.");
            txtRuc.Focus();
            return;
        }

        Almacen.Transportistas.Add(new Transportista
        {
            Id = Almacen.SiguienteId(Almacen.Transportistas, t => t.Id),
            Nombre = txtNombre.Text.Trim(),
            Ruc = ruc,
            Telefono = txtTelefono.Text.Trim()
        });

        MessageBox.Show("Transportista registrado correctamente.", "Mantenimiento de Transportistas",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        Limpiar();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (dgTransportistas.SelectedItem is not Transportista t)
        {
            Aviso("Seleccione un transportista de la lista.");
            return;
        }

        if (MessageBox.Show($"Desea eliminar a {t.Nombre}?", "Confirmar",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

        Almacen.Transportistas.Remove(t);
    }

    private static void Aviso(string mensaje)
        => MessageBox.Show(mensaje, "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
}
