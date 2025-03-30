using System.Net.Sockets;
using System.Net;
using P2P_Chat;

namespace lab3 {
    internal class UDPClient {
        Socket socket;
        int port;
        UserInfo user;
        MessageManager msg;
        public void SendMessage() {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            socket.Bind(new IPEndPoint(user.ip, 0));

            socket.SendTo(msg.CreateUserInfoMessage(user), new IPEndPoint(new IPAddress([255, 255, 255, 255]), port));
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public UDPClient(int port, UserInfo user, MessageManager msg) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.port = port;
            this.user = user;
            this.msg = msg;
        }
    }
}
