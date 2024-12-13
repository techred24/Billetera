using MPOST;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        //private IDocument billete;



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
            // Subscripcion al evento
            acceptor.OnStackedWithDocInfo += HandleStackedWithDocInfo;
            acceptor.OnReturned += Acceptor_OnReturned;

        }



        // Eventos
        private void Acceptor_OnReturned(object sender, EventArgs e)
        {
            //MessageBox.Show($"TIPO: {e.ToString()}");
            billLabel.Dispatcher.Invoke(() =>
            {
                billLabel.Content = $"TIPO: {e.ToString()}";
            });
        }

        private void HandleStackedWithDocInfo(object sender, EventArgs e)
        {
            //MessageBox.Show($"TIPO: {e.ToString()}");
            billLabel.Dispatcher.Invoke(() =>
            {
                billLabel.Content = $"TIPO: {e.ToString()}";
            });
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
                    
                    //Bill? bill = acceptor.getDocument() as Bill;

                }
            }

        }


        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (acceptor.DeviceState == State.Escrow)
            {
                
                //billete = acceptor.getDocument();
                Bill? bill = acceptor.getDocument() as Bill;

                if (bill != null)
                {
                   // MessageBox.Show($"País: {bill.Country}");
                    MessageBox.Show($"Valor del Billete: {bill.Value}");
                }
                //MessageBox.Show(billete.ValueString);
                acceptor.EscrowReturn();
                
            }
        }

        private void btnStack_Click(object sender, RoutedEventArgs e)
        {
            if(acceptor.DeviceState == State.Escrow)
            {
                //billete = acceptor.getDocument();
                Bill? bill = acceptor.getDocument() as Bill;
                if (bill != null) { 
                    //MessageBox.Show($"País: {bill.Country}");
                    MessageBox.Show($"Valor del Billete: {bill.Value}");
                }
                //MessageBox.Show(billete.ValueString);
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
