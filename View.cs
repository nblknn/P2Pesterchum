using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using lab3;

namespace P2P_Chat {
    internal class View {
        private StackPanel container;
        public Color[] colors = {
            Color.FromRgb(0xa1, 0x00, 0x00), Color.FromRgb(0xa1, 0x50, 0x00), Color.FromRgb(0xa1, 0xa1, 0x00), Color.FromRgb(0x62, 0x62, 0x62),
            Color.FromRgb(0x41, 0x66, 0x00), Color.FromRgb(0x00, 0x81, 0x41), Color.FromRgb(0x00, 0x82, 0x82), Color.FromRgb(0x00, 0x56, 0x82),
            Color.FromRgb(0x00, 0x00, 0x56), Color.FromRgb(0x42, 0x00, 0xb0), Color.FromRgb(0x6a, 0x00, 0x6a),  Color.FromRgb(0x77, 0x00, 0x3c),
            Color.FromRgb(0x07, 0x15, 0xcd), Color.FromRgb(0x00, 0xd5, 0xf2), Color.FromRgb(0xb5, 0x36, 0xda),  Color.FromRgb(0xff, 0x6f, 0xf2),
            Color.FromRgb(0xe0, 0x07, 0x07), Color.FromRgb(0xf2, 0xa4, 0x00), Color.FromRgb(0x4a, 0xc9, 0x25),  Color.FromRgb(0x1f, 0x94, 0x00),};
        public View(StackPanel stackPanel) {
            container = stackPanel;
        }

        public Color AssignColor(String name) {
            return colors[Math.Abs(name.GetHashCode()) % colors.Length];
        }

        public void ShowMessage(UserInfo user, String message) {
            container.Dispatcher.Invoke(() => {
                TextBlock tb = new TextBlock() {
                    Text = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + user.name + " (" + user.ip + "): " + message,
                    Foreground = new SolidColorBrush(AssignColor(user.name)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 1, 10, 1),
                };
                container.Children.Add(tb);
                tb.BringIntoView();
            });
        }

        public void ShowOwnerMessage(String message) {
            container.Dispatcher.Invoke(() => {
                container.Children.Add(new TextBlock() {
                    Text = message,
                    HorizontalAlignment = HorizontalAlignment.Right,
                });
            });
        }

        public void ShowNewUserMessage(UserInfo user) {
            container.Dispatcher.Invoke(() => {
                TextBlock tb = new TextBlock() {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 1, 10, 1)
                };
                tb.Inlines.Add(new Run("-- Пользователь ") {
                    Foreground = Brushes.Black
                });
                tb.Inlines.Add(new Run(user.name + " (" + user.ip + ")") {
                    Foreground = new SolidColorBrush(AssignColor(user.name)),
                });
                tb.Inlines.Add(new Run(" присоединился к чату в " + DateTime.Now.ToString("HH:mm:ss") + " --") {
                    Foreground = Brushes.Black
                });
                container.Children.Add(tb);
                tb.BringIntoView();
            });
        }

        public void ShowUserExitMessage(UserInfo user) {
            container.Dispatcher.Invoke(() => {
                TextBlock tb = new TextBlock() {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 1, 10, 1)
                };
                tb.Inlines.Add(new Run("-- Пользователь ") {
                    Foreground = Brushes.Black
                });
                tb.Inlines.Add(new Run(user.name + " (" + user.ip + ")") {
                    Foreground = new SolidColorBrush(AssignColor(user.name)),
                });
                tb.Inlines.Add(new Run(" покинул чат в " + DateTime.Now.ToString("HH:mm:ss") + " --") {
                    Foreground = Brushes.Black
                });
                container.Children.Add(tb);
                tb.BringIntoView();
            });
        }
    }
}
