using System.Net.Sockets;
using System.Net;
using P2P_Chat;

namespace lab3 {
    internal class TCPServer {
        List<TCPClient> clients;
        Socket socket;
        int port;
        IPAddress ip;
        View view;
        volatile bool isRunning;
        MessageManager msg;
        private void ProcessClient(object? clientSocket) {
            int bytesread = 0;
            byte[] buf = new byte[MainWindow.MAXNAMELENGTH];
            bytesread = (clientSocket as Socket).Receive(buf, 0);
            UserInfo clientInfo = msg.GetUserInfo(buf, bytesread);
            clientInfo.SetIP(((clientSocket as Socket).RemoteEndPoint as IPEndPoint).Address);
            TCPClient tcpclient = new TCPClient(clientSocket as Socket, clientInfo, view, msg);
            clients.Add(tcpclient);
            tcpclient.ReceiveMessages(clients);
        }

        public void WaitForConnections() {
            isRunning = true;
            socket.Bind(new IPEndPoint(ip, port));
            while (isRunning) {
                socket.Listen();
                Socket newclient = socket.Accept();
                Thread newthread = new Thread(new ParameterizedThreadStart(ProcessClient)) {
                    IsBackground = true
                };
                newthread.Start(newclient);
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public void StopRunning() {
            isRunning = false;
        }

        public TCPServer(int port, IPAddress ip, View view, List<TCPClient> clients, MessageManager msg) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.port = port;
            this.ip = ip;
            this.view = view;
            this.msg = msg;
            this.clients = clients;
        }

    }
}
