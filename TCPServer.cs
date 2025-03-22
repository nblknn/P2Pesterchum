using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using P2P_Chat;

namespace lab3 {
    internal class TCPServer {
        Socket socket;
        volatile List<Socket> clients = new List<Socket>();
        int port;
        IPAddress ip;
        View view;
        private void ProcessClient(object? client) {
            int bytesread = 0;
            byte[] buf = new byte[MainWindow.MAXNAMELENGTH];
            bytesread = (client as Socket).Receive(buf, 0);
            UserInfo clientInfo = new UserInfo(Encoding.Default.GetString(buf, 0, bytesread),
                ((client as Socket).RemoteEndPoint as IPEndPoint).Address);
            buf = new byte[1024];
            try {
                while ((bytesread = (client as Socket).Receive(buf, 0)) > 0) {
                    view.ShowMessage(clientInfo, Encoding.Default.GetString(buf, 0, bytesread));
                    Array.Clear(buf, 0, bytesread);
                }
            }
            catch (System.Net.Sockets.SocketException) { }
            finally {
                view.ShowUserExitMessage(clientInfo);
                (client as Socket).Shutdown(SocketShutdown.Both);
                (client as Socket).Close();
                clients.Remove(client as Socket);  
            }
        }

        public void SendMessage(String msg) {
            foreach (Socket socket in clients) {
                socket.Send(Encoding.Default.GetBytes(msg));
            }
        }

        public void WaitForConnections() {
            socket.Bind(new IPEndPoint(ip, port));
            while (true) {
                socket.Listen();
                Socket newclient = socket.Accept();
                Thread newthread = new Thread(new ParameterizedThreadStart(ProcessClient)) {
                    IsBackground = true
                };
                newthread.Start(newclient);
                clients.Add(newclient);
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public TCPServer(int port, IPAddress ip, View view) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.port = port;
            this.ip = ip;
            this.view = view;
        }

    }
}
