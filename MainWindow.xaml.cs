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
using System.IO.Ports;

namespace PromacWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort? _serial;

        public MainWindow()
        {
            InitializeComponent();
            comboBoxParity.ItemsSource = Enum.GetValues(typeof(Parity)).Cast<Parity>().ToList();
            comboBoxStopBit.ItemsSource = Enum.GetValues(typeof(StopBits)).Cast<StopBits>().ToList();
            comboBoxFlowControl.ItemsSource = Enum.GetValues(typeof(Handshake)).Cast<Handshake>().ToList();
        }

        private void buttonRefreshPort_Click(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length == 0 || ports == null)
            {
                MessageBox.Show("No ports found.", " Port error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            comboBoxComPort.Items.Clear();

            foreach (string port in ports)
            {
                comboBoxComPort.Items.Add(port);
            }

            comboBoxComPort.SelectedIndex = 0;

        }

        private void buttonOpenPort_Click(object sender, RoutedEventArgs e)
        {


            if (comboBoxComPort.Text.Equals(""))
            {
                MessageBox.Show("No port provided.", "Port error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_serial == null)
            {
                 _serial = new SerialPort();
            }

            if (_serial.IsOpen)
            {
                MessageBox.Show($"Port {_serial.PortName} already open.", "Port error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _serial.PortName = comboBoxComPort.Text;
            _serial.BaudRate = Int32.Parse(comboBoxRate.Text);
            _serial.DataBits = Int32.Parse(comboBoxDataBit.Text);
            _serial.Parity = (Parity)comboBoxParity.SelectedIndex;
            _serial.StopBits = (StopBits)comboBoxStopBit.SelectedIndex;
            _serial.Handshake = (Handshake)comboBoxFlowControl.SelectedIndex;

            _serial.Open();
            
            if (_serial.IsOpen)
            {
                MessageBox.Show($"Successfully opened {_serial.PortName} port.", "Port opened", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void buttonClosePort_Click(object sender, RoutedEventArgs e)
        {
            if (_serial == null)
            {
                MessageBox.Show("Port is not initialized.", "Port error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_serial.IsOpen)
            {
                MessageBox.Show($"Port {_serial.PortName} is already closed.", "Port error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _serial.Close();

            if (!_serial.IsOpen)
            {
                MessageBox.Show($"Successfully closed {_serial.PortName} port.", "Port closed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonRestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            comboBoxComPort.SelectedIndex = 0;
            comboBoxRate.SelectedIndex = 12;
            comboBoxDataBit.SelectedIndex = 4;
            comboBoxParity.SelectedIndex = 0;
            comboBoxStopBit.SelectedIndex = 1;
            comboBoxFlowControl.SelectedIndex = 0;
        }

    }
}
