using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using lab3;

namespace P2P_Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        const int PORT = 1024;
        public const int MAXNAMELENGTH = 44;
        UserInfo user;

        UDPClient udpclient;
        UDPServer udpserver;
        TCPServer tcpserver;
        List<TCPClient> tcpclients;
        MessageManager msg;

        View view;

        public MainWindow() {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));

            UserInfoWindow userInfoWindow = new UserInfoWindow();
            if (userInfoWindow.ShowDialog() == false) this.Close();
            else user = userInfoWindow.user;
            tbUserName.Text = ":: " + user.name + " ::";
            Title += " (" + user.name + ")";

            view = new View(spMessages);
            msg = new MessageManager();
            user.SetColor(view.AssignColor(user.name));

            tcpclients = new List<TCPClient>();
            udpclient = new UDPClient(PORT, user, msg);
            udpserver = new UDPServer(PORT, user, view, msg);
            tcpserver = new TCPServer(PORT, user.ip, view, tcpclients, msg);
            Thread tcpserverthread = new Thread(tcpserver.WaitForConnections) {
                IsBackground = true
            };
            Thread udpserverthread = new Thread(new ParameterizedThreadStart(udpserver.ListenForNewUsers)) {
                IsBackground = true
            };

            tcpserverthread.Start();
            udpclient.SendMessage();
            udpserverthread.Start(tcpclients);
            view.ShowNewUserMessage(user);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (tbMessage.Text.Length == 0) return;
            byte[] message = msg.CreateTextMessage(tbMessage.Text);
            foreach (TCPClient client in tcpclients) {
                client.SendMessage(message);
            }
            view.ShowTextMessage(user, tbMessage.Text);
            tbMessage.Text = "";
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Button_Click(sender, e);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            byte[] message = msg.CreateDisconnectMessage();
            udpserver.StopRunning();
            foreach (TCPClient client in tcpclients) {
                client.SendMessage(message);
                client.StopRunning();
            }
            tcpserver.StopRunning();
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e) {
            SystemCommands.CloseWindow(this);
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e) {
            SystemCommands.MaximizeWindow(this);
        }

        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e) {
            SystemCommands.MinimizeWindow(this);
        }
    }
}