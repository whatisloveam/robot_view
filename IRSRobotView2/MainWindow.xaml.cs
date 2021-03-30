using System;
using System.Collections.Generic;
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

namespace IRSRobotView2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort myPort;
        string[] ports;
        public MainWindow()
        {
            InitializeComponent();

            myPort = new SerialPort();
            myPort.DataReceived += MyPort_DataReceived;
            myPort.ReadTimeout = 2000;
            myPort.WriteTimeout = 2000;

            SendButton.IsEnabled = false;
            DisconnectButton.IsEnabled = false;
            ports = SerialPort.GetPortNames();
            comboBox.ItemsSource = ports;
            comboBox.SelectedValue = "COM7";
        }

        private void MyPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string readstring;
            try
            {
                readstring = myPort.ReadLine();
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    textBox.AppendText(readstring + "\n");
                    textBox.ScrollToEnd();
                }));
            }
            catch (TimeoutException)
            {

            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            myPort.PortName = (string)comboBox.SelectedItem;
            myPort.BaudRate = 57600;
            try
            {
                textBox.Clear();
                myPort.Open();
                ConnectButton.IsEnabled = false;
                DisconnectButton.IsEnabled = true;
                SendButton.IsEnabled = true;
            }
            catch (InvalidOperationException)
            {

            }
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myPort.Close();
                ConnectButton.IsEnabled = true;
                DisconnectButton.IsEnabled = false;
                SendButton.IsEnabled = false;
            }
            catch (InvalidOperationException)
            {

            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string writestring;
            writestring = textBox1.Text;
            try
            {
                myPort.WriteLine(String.Format("{0}", writestring));
                textBox1.Clear();
            }
            catch (TimeoutException)
            {

            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
        }

    }
}
