using lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace P2P_Chat {
    /// <summary>
    /// Логика взаимодействия для UserInfoWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window {
        internal UserInfo user;

        public UserInfoWindow() {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (tbName.Text.Length == 0 || tbIP.Text.Length == 0) return;
            user = new UserInfo(tbName.Text, IPAddress.Parse(tbIP.Text));
            DialogResult = true;
            Close();
        }

        private void tbName_KeyDown(object sender, KeyEventArgs e) {
            if (Encoding.Default.GetByteCount(tbName.Text) >= MainWindow.MAXNAMELENGTH && e.Key != Key.Back)
                e.Handled = true;
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
