using MPOST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Billetera
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page
    {
        private static double totalDepositar = 50;
        private Acceptor? acceptor;
        public Principal()
        {
            InitializeComponent();
            // Método de double: ingresado.ToString("F2");
            //string depositadoTexto = Depositado.Text.Replace("$", "");
            //float.TryParse(depositadoTexto, out float importeatm);
            try
            {
                acceptor = new Acceptor();
                acceptor.Open("COM2");
                TotalDepositar.Text = $"${totalDepositar:0.00}";
                Restante.Text = $"${totalDepositar:0.00}";
                //Depositado.Text = $"${totalDepositar:0.00}";
            } catch (Exception ex)
            {
                MessageBox.Show($"Error en el constructor de Principal: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }


        }
        //private void DevolverButton(object sender, RoutedEventArgs e)
        //{
        //    //acceptor.EscrowReturn();
        //    //acceptor.Close();
        //}
        //private void Stack(object sender, RoutedEventArgs e)
        //{
        //    //acceptor.EscrowStack();
        //    //acceptor.Close();
        //}
    }
}
