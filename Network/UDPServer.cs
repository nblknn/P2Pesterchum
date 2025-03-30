using System.Net.Sockets;
using System.Net;
using P2P_Chat;

namespace lab3 {
    internal class UDPServer {
        Socket socket;
        int port;
        UserInfo user;
        View view;
        MessageManager msg;
        volatile bool isRunning;
        public void ListenForNewUsers(object? clientlist) {
            isRunning = true;
            int bytesread = 0;
            socket.Bind(new IPEndPoint(user.ip, port));
            byte[] buf = new byte[MainWindow.MAXNAMELENGTH];
            EndPoint endPoint = new IPEndPoint(user.ip, 0);
            while (isRunning) {
                bytesread = socket.ReceiveFrom(buf, ref endPoint);
                UserInfo newuser = msg.GetUserInfo(buf, bytesread);
                newuser.SetIP((endPoint as IPEndPoint).Address);
                TCPClient tcpclient = new TCPClient(port, newuser, user, view, msg);
                (clientlist as List<TCPClient>).Add(tcpclient);
                Thread thread = new Thread(new ParameterizedThreadStart(tcpclient.ConnectToServer)) {
                    IsBackground = true
                };
                thread.Start(clientlist);
                Array.Clear(buf, 0, bytesread);
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public void StopRunning() {
            isRunning = false;
        }

        public UDPServer(int port, UserInfo user, View view, MessageManager msg) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.port = port;
            this.user = user;
            this.view = view;
            this.msg = msg;
        }
    }
}
