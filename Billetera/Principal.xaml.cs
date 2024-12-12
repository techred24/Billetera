using MPOST;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
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
        private Acceptor acceptor;
        private IDocument billete;
        public Principal()
        {
            InitializeComponent();
            // Método de double: ingresado.ToString("F2");
            //string depositadoTexto = Depositado.Text.Replace("$", "");
            //float.TryParse(depositadoTexto, out float importeatm);
            //try
            //{
            
            acceptor = new Acceptor();
            Thread threadAcceptor = new Thread(AcceptorRoutine);
            threadAcceptor.IsBackground = true;
            threadAcceptor.Start();
            btnReturn.IsEnabled = false;
            btnStack.IsEnabled = false;

        }

        private void AcceptorRoutine()
        {
            ledStatus.Dispatcher.Invoke(() =>
            { ledStatus.Fill = new SolidColorBrush(Colors.Red); });
            acceptor.Open("COM2",PowerUp.A);
            // Con este delay no marca error de conexion
            Thread.Sleep(6000);

            while (true)
            {
                if (acceptor.Connected)
                {
                    acceptor.EnableAcceptance = true;
                    btnReturn.Dispatcher.Invoke(() => { btnReturn.IsEnabled = true; });
                    btnStack.Dispatcher.Invoke(() => { btnStack.IsEnabled = true; });
                    ledStatus.Dispatcher.Invoke(() =>
                    { ledStatus.Fill = new SolidColorBrush(Colors.Green); });
                }
            }

        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (acceptor.DeviceState == State.Escrow)
            {
                billete = acceptor.getDocument();
                MessageBox.Show(billete.ValueString);
                acceptor.EscrowReturn();
            }
        }

        private void btnStack_Click(object sender, RoutedEventArgs e)
        {
            if(acceptor.DeviceState == State.Escrow)
            {
                billete = acceptor.getDocument();
                MessageBox.Show(billete.ValueString);
                acceptor.EscrowStack();
                
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
