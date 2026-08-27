using System.Windows;
using System.Windows.Input;
using lab2.Datos;

namespace lab2.Vistas;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        txtUsuario.Focus();
    }

    private void btnIngresar_Click(object sender, RoutedEventArgs e) => Autenticar();

    private void txtPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) Autenticar();
    }

    private void Autenticar()
    {
        var usuario = txtUsuario.Text.Trim();
        var password = txtPassword.Password;

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
        {
            MostrarError("Debe ingresar el usuario y la contrasena.");
            return;
        }

        var encontrado = Almacen.Validar(usuario, password);
        if (encontrado is null)
        {
            MostrarError("Usuario o contrasena incorrectos.");
            MessageBox.Show("Usuario o contrasena incorrectos.\n\nVerifique sus credenciales e intente nuevamente.",
                            "Validacion", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtPassword.Clear();
            txtPassword.Focus();
            return;
        }

        Almacen.UsuarioActual = encontrado;
        lblError.Visibility = Visibility.Collapsed;

        var menu = new MenuWindow();
        Application.Current.MainWindow = menu;
        menu.Show();
        Close();
    }

    private void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.Visibility = Visibility.Visible;
    }

    private void btnSalir_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
}
