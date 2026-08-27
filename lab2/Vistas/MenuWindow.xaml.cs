using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lab2.Datos;

namespace lab2.Vistas;

public partial class MenuWindow : Window
{
    public MenuWindow()
    {
        InitializeComponent();
        lblUsuario.Text = Almacen.UsuarioActual?.NombreCompleto ?? "Invitado";
        lblFecha.Text = DateTime.Now.ToString("dddd dd 'de' MMMM 'de' yyyy");
        CargarResumen();
    }

    private void CargarResumen()
    {
        pnlResumen.Children.Clear();
        AgregarTarjeta("Ingresos registrados", Almacen.Ingresos.Count.ToString(), "#1E88E5");
        AgregarTarjeta("Salidas registradas",  Almacen.Salidas.Count.ToString(),  "#43A047");
        AgregarTarjeta("Conductores",          Almacen.Conductores.Count.ToString(), "#FB8C00");
        AgregarTarjeta("Transportistas",       Almacen.Transportistas.Count.ToString(), "#8E24AA");
        AgregarTarjeta("Camiones",             Almacen.Camiones.Count.ToString(), "#00897B");
        AgregarTarjeta("Productos",            Almacen.Productos.Count.ToString(), "#5E35B1");
    }

    private void AgregarTarjeta(string titulo, string valor, string color)
    {
        var borde = new Border
        {
            Width = 170,
            Height = 80,
            Margin = new Thickness(0, 0, 12, 12),
            CornerRadius = new CornerRadius(6),
            Background = Brushes.White,
            BorderBrush = (Brush)FindResource("BrushBorde"),
            BorderThickness = new Thickness(1)
        };

        var panel = new StackPanel { Margin = new Thickness(14, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
        panel.Children.Add(new TextBlock
        {
            Text = valor,
            FontSize = 26,
            FontWeight = FontWeights.Bold,
            Foreground = (Brush)new BrushConverter().ConvertFrom(color)!
        });
        panel.Children.Add(new TextBlock { Text = titulo, FontSize = 12, Foreground = Brushes.Gray });

        borde.Child = panel;
        pnlResumen.Children.Add(borde);
    }

    private void Abrir(Window ventana, string estado)
    {
        lblEstado.Text = estado;
        ventana.Owner = this;
        ventana.ShowDialog();
        CargarResumen();
        lblEstado.Text = "Listo";
    }

    // ---------- Operaciones ----------
    private void MenuIngresos_Click(object sender, RoutedEventArgs e) => Abrir(new IngresoWindow(), "Registro de ingresos");
    private void MenuSalida_Click(object sender, RoutedEventArgs e) => Abrir(new SalidaWindow(), "Registro de salidas");

    // ---------- Mantenimientos ----------
    private void MenuConductores_Click(object sender, RoutedEventArgs e) => Abrir(new ConductorWindow(), "Mantenimiento de conductores");
    private void MenuTransportistas_Click(object sender, RoutedEventArgs e) => Abrir(new TransportistaWindow(), "Mantenimiento de transportistas");
    private void MenuCamiones_Click(object sender, RoutedEventArgs e) => Abrir(new CamionWindow(), "Mantenimiento de camiones");
    private void MenuProductos_Click(object sender, RoutedEventArgs e) => Abrir(new ProductoWindow(), "Mantenimiento de productos");

    // ---------- Reportes ----------
    private void MenuRepCargas_Click(object sender, RoutedEventArgs e) => Abrir(new ReporteCargasWindow(), "Reporte de cargas");
    private void MenuRepIngresos_Click(object sender, RoutedEventArgs e) => Abrir(new ReporteIngresosWindow(), "Reporte de ingresos");
    private void MenuRepSalidas_Click(object sender, RoutedEventArgs e) => Abrir(new ReporteSalidasWindow(), "Reporte de salidas");

    // ---------- Sesion ----------
    private void MenuCerrarSesion_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Desea cerrar la sesion actual?", "Confirmar",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

        Almacen.UsuarioActual = null;
        var login = new LoginWindow();
        Application.Current.MainWindow = login;
        login.Show();
        Close();
    }

    private void MenuSalir_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

    private void Acceso_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Button)?.Tag?.ToString();
        switch (tag)
        {
            case "ingreso":  Abrir(new IngresoWindow(), "Registro de ingresos"); break;
            case "salida":   Abrir(new SalidaWindow(), "Registro de salidas"); break;
            case "conductor":Abrir(new ConductorWindow(), "Mantenimiento de conductores"); break;
            case "lista":    Abrir(new ConductorListaWindow(), "Lista de conductores"); break;
            case "transp":   Abrir(new TransportistaWindow(), "Mantenimiento de transportistas"); break;
            case "camion":   Abrir(new CamionWindow(), "Mantenimiento de camiones"); break;
            case "producto": Abrir(new ProductoWindow(), "Mantenimiento de productos"); break;
            case "reping":   Abrir(new ReporteIngresosWindow(), "Reporte de ingresos"); break;
            case "repsal":   Abrir(new ReporteSalidasWindow(), "Reporte de salidas"); break;
            case "repcar":   Abrir(new ReporteCargasWindow(), "Reporte de cargas"); break;
        }
    }
}
