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
        public static double totalDepositar = 50;
        private Acceptor acceptor;
        public Principal()
        {
            InitializeComponent();
            // Método de double: ingresado.ToString("F2");
            //string depositadoTexto = Depositado.Text.Replace("$", "");
            //float.TryParse(depositadoTexto, out float importeatm);
            acceptor = new Acceptor();
            acceptor.Open("COM2");
            TotalDepositar.Text = $"${totalDepositar:0.00}";
            Restante.Text = $"${totalDepositar:0.00}";
            //Depositado.Text = $"${totalDepositar:0.00}";
        }
        private async Task DevolverButton(object sender, RoutedEventArgs e)
        {
            acceptor.EscrowReturn();
            await Task.Delay(2000);
            acceptor.Close();
        }
        private async Task Stack(object sender, RoutedEventArgs e)
        {
            acceptor.EscrowStack();
            await Task.Delay(2000);
            acceptor.Close();
        }
    }
}
