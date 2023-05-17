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
using System.Runtime.InteropServices;
using System.Threading;

namespace PromacWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32", SetLastError = true)]
        public static extern void FreeConsole();

        private SerialPort? _serial;
        private Thread _readSerial;


        public MainWindow()
        {
            InitializeComponent();
            comboBoxParity.ItemsSource = Enum.GetValues(typeof(Parity)).Cast<Parity>().ToList();
            comboBoxStopBit.ItemsSource = Enum.GetValues(typeof(StopBits)).Cast<StopBits>().ToList();
            comboBoxFlowControl.ItemsSource = Enum.GetValues(typeof(Handshake)).Cast<Handshake>().ToList();
            AllocConsole();
        }

        private void ReadSerial()
        {

            if (_serial ==  null)
            {
                return;
            }

            try
            {
                while (true)
                {
                    var s = _serial.ReadLine();
                    if (s != null)
                    {
                        Console.Write(s.Trim() + "\r\n");
                    }
                }
            } catch (Exception ex)
            {
                return;
            }
        }

        private void ProcessByte(string s)
        {
            if (_serial == null || !_serial.IsOpen)
            {
                MessageBox.Show("Invalid handle.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _serial.Write(s);
        }

        private void TestPort()
        {
            string s = "0123456789ABCDEF";
            ProcessByte(s);
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

        private void buttonTestPort_Click(object sender, RoutedEventArgs e)
        {
            TestPort();
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

            try
            {
                _serial.Open();
                _readSerial = new Thread(() => { ReadSerial(); });
                _readSerial.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured.\nStacktrace:\n{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
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

            try
            {
                _serial.Close();
                _readSerial.Interrupt();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured.\nStacktrace:\n{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

        private void buttonF_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("F");
        }

        private void buttonE_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("E");
        }

        private void buttonD_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("D");
        }

        private void buttonC_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("C");
        }

        private void buttonB_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("B");
        }

        private void buttonA_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("A");
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("9");
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("8");
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("7");
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("6");
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("5");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("4");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("3");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("2");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("1");
        }

        private void button0_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("0");
        }

        private void buttonDevice_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("P");
        }

        private void buttonFunction_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("U");
        }

        private void buttonRomType_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("R");
        }

        private void buttonUpArrow_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte(" ");
        }

        private void buttonDownButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("/");
        }

        private void buttonSet_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("\r");
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("@");
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("T");
        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuItemBlankCheck_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("PC\r");
        }

        private void menuItemCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemProgram_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemVerify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemRamClean_Click(object sender, RoutedEventArgs e)
        {
            ProcessByte("T4\r");
        }
    }
}
