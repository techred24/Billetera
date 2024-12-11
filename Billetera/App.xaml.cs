using System.Configuration;
using System.Data;
using System.Windows;

namespace Billetera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            //{
            //    MessageBox.Show($"Excepción no controlada: {args.ExceptionObject}");
            //};

            //DispatcherUnhandledException += (sender, args) =>
            //{
            //    MessageBox.Show($"Excepción no controlada en el hilo: {args.Exception.Message}");
            //    args.Handled = true; // Evita que la aplicación se cierre
            //};

            //base.OnStartup(e);
            //MessageBox.Show("La aplicación se está iniciando");
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
        }

    }

}
