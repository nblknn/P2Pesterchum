using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using P2P_Chat;

namespace lab3 {
    internal class UDPServer {
        Socket socket;
        int port;
        UserInfo user;
        View view;
        public void ListenForNewUsers(object? clientlist) {
            int bytesread = 0;
            socket.Bind(new IPEndPoint(user.ip, port));
            byte[] buf = new byte[MainWindow.MAXNAMELENGTH];
            EndPoint endPoint = new IPEndPoint(user.ip, 0);
            while (true) {
                bytesread = socket.ReceiveFrom(buf, ref endPoint);
                TCPClient tcpclient = new TCPClient(port,
                    new UserInfo(Encoding.Default.GetString(buf, 0, bytesread), (endPoint as IPEndPoint).Address), user, view);
                (clientlist as List<TCPClient>).Add(tcpclient);
                Thread thread = new Thread(new ParameterizedThreadStart(tcpclient.ReceiveMessages)) {
                    IsBackground = true
                };
                thread.Start(clientlist);
                Array.Clear(buf, 0, bytesread);
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public UDPServer(int port, UserInfo user, View view) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.port = port;
            this.user = user;
            this.view = view;
        }
    }
}
