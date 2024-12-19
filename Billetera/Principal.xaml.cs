using MPOST;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Billetera
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page
    {
        private static double totalDepositar = 70;
        private static double restante = totalDepositar;
        private static double depositado = 0;
        private Acceptor acceptor;
        private Bill? billete ;
        private delegate void ReturnedDelegate(object sender, EventArgs e);
        private System.Timers.Timer _timer;
        private System.Threading.Timer _theTimer;
        private DispatcherTimer _dispatcherTimer;
        private static int stackedCouter = 1;
        public Principal()
        {
            InitializeComponent();
            // Método de double: ingresado.ToString("F2");
            //string depositadoTexto = Depositado.Text.Replace("$", "");
            //float.TryParse(depositadoTexto, out float importeatm);
            //try
            //{
            //$"${ingresado:00}";
            TotalDepositar.Text = $"${totalDepositar:0.##}";
            Depositado.Text = $"${depositado:0.##}";
            Restante.Text = $"{restante:0.##}";
            
            acceptor = new Acceptor();
            Thread threadAcceptor = new Thread(AcceptorRoutine);
            //Thread eventsThread = new Thread(FireEventsRoutine);
            threadAcceptor.IsBackground = true;
            threadAcceptor.Start();
            btnReturn.IsEnabled = false;
            btnStack.IsEnabled = false;

            // Subscripcion al evento
            acceptor.OnStackedWithDocInfo += HandleStackedWithDocInfo;
            acceptor.OnReturned += Acceptor_OnReturned;
            
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (acceptor.DeviceState == State.Escrow)
            {
                billete = acceptor.getDocument() as Bill;

                if (billete != null)
                {
                    string depositadoTexto = Depositado.Text.Replace("$", "");
                    double.TryParse(depositadoTexto, out double depositadoDouble);

                    double newDepositadoValue = depositadoDouble + billete.Value;
                    double newRestanteValue = totalDepositar - newDepositadoValue;

                    if (newDepositadoValue > totalDepositar)
                    {
                        acceptor.EscrowReturn();
                        //MessageBox.Show("Se pagó de más");
                        return;
                    }
                    acceptor.EscrowStack();
                    Depositado.Text = $"${newDepositadoValue:0.##}";
                    Restante.Text = $"${newRestanteValue:0.##}";
                }
            }
        }


        private void Acceptor_OnReturned(object sender, EventArgs e)
        {
            MessageBox.Show("Billete regresado");
        }

        private void HandleStackedWithDocInfo(object sender, EventArgs e)
        {
            totalDepositar += billete.Value;
            Depositado.Text = totalDepositar.ToString();
        }

        private  void AcceptorRoutine()
        {
            ledStatus.Dispatcher.Invoke(() =>
            { ledStatus.Fill = new SolidColorBrush(Colors.Red); });
            acceptor.Open("COM2",PowerUp.A);
            // Con este delay no marca error de conexion
            Thread.Sleep(6000);

            //Esta linea es la que manda a disparar el evento
            //Acceptor_OnConnected(this, EventArgs.Empty);

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
           
                //Bill? bill = acceptor.getDocument() as Bill;
                billete = acceptor.getDocument() as Bill;

                if (billete != null)
                {
                    acceptor.EscrowReturn();
                    Acceptor_OnReturned(this, EventArgs.Empty);
                }
            }

        }

        private void btnStack_Click(object sender, RoutedEventArgs e)
        {
            if(acceptor.DeviceState == State.Escrow)
            {
                billete = acceptor.getDocument() as Bill;
                //Bill? bill = acceptor.getDocument() as Bill;
                if (billete != null) {
                    //MessageBox.Show($"País: {bill.Country}");
                   
                    acceptor.EscrowStack();
                    HandleStackedWithDocInfo(this, EventArgs.Empty);
                }
            }
        }
    }
}
