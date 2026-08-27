using System.Windows;
using lab2.Datos;
using lab2.Modelos;

namespace lab2.Vistas;

public partial class ProductoWindow : Window
{
    public ProductoWindow()
    {
        InitializeComponent();
        cboUnidad.ItemsSource = new[] { "KG", "TN", "LT", "UND" };
        dgProductos.ItemsSource = Almacen.Productos;
        Limpiar();
    }

    private void Limpiar()
    {
        txtNombre.Clear();
        cboUnidad.SelectedIndex = 0;
        txtNombre.Focus();
    }

    private void btnNuevo_Click(object sender, RoutedEventArgs e) => Limpiar();

    private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            Aviso("Ingrese el nombre del producto.");
            txtNombre.Focus();
            return;
        }

        if (Almacen.Productos.Any(p => string.Equals(p.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
        {
            Aviso($"El producto {nombre} ya se encuentra registrado.");
            txtNombre.Focus();
            return;
        }

        var unidad = cboUnidad.Text.Trim();
        if (string.IsNullOrWhiteSpace(unidad))
        {
            Aviso("Seleccione o ingrese la unidad de medida.");
            cboUnidad.Focus();
            return;
        }

        Almacen.Productos.Add(new Producto
        {
            Id = Almacen.SiguienteId(Almacen.Productos, p => p.Id),
            Nombre = nombre,
            Unidad = unidad.ToUpperInvariant()
        });

        MessageBox.Show("Producto registrado correctamente.", "Mantenimiento de Productos",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        Limpiar();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (dgProductos.SelectedItem is not Producto p)
        {
            Aviso("Seleccione un producto de la lista.");
            return;
        }

        if (MessageBox.Show($"Desea eliminar el producto {p.Nombre}?", "Confirmar",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

        Almacen.Productos.Remove(p);
    }

    private static void Aviso(string mensaje)
        => MessageBox.Show(mensaje, "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
}
