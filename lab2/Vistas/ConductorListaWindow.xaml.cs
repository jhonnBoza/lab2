using System.Windows;
using System.Windows.Controls;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class ConductorListaWindow : Window
{
    public ConductorListaWindow()
    {
        InitializeComponent();
        Cargar();
    }

    private void Cargar()
    {
        var filtro = txtBuscar.Text.Trim();
        var lista = Almacen.Conductores.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            lista = lista.Where(c =>
                c.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                c.Licencia.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                c.Transporte.Contains(filtro, StringComparison.OrdinalIgnoreCase));
        }

        var resultado = lista.OrderBy(c => c.Nombre).ToList();
        dgConductores.ItemsSource = resultado;
        lblTotal.Text = $"Total de conductores: {resultado.Count}";
    }

    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e) => Cargar();

    private void btnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        txtBuscar.Clear();
        Cargar();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (dgConductores.SelectedItem is not Conductor c)
        {
            MessageBox.Show("Seleccione un conductor de la lista.", "Validacion",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (MessageBox.Show($"Desea eliminar al conductor {c.Nombre}?", "Confirmar",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

        Almacen.Conductores.Remove(c);
        Cargar();
    }

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();
}
