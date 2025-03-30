using System.Net.Sockets;
using System.Net;
using P2P_Chat;

namespace lab3 {
    internal class TCPClient {
        Socket socket;
        int remotePort;
        UserInfo remoteUserInfo;
        UserInfo localUserInfo;
        View view;
        MessageManager msg;
        volatile bool isRunning;

        public void SendMessage(object? msg) {
            socket.Send(msg as byte[]);
        }

        public void ReceiveMessages(List<TCPClient> clientlist) {
            int bytesread = 0;
            isRunning = true;
            byte[] buf = new byte[1024];
            try {
                while (((bytesread = socket.Receive(buf, 0)) > 0) && isRunning) {
                    view.ShowMessage(remoteUserInfo, buf, bytesread);
                    if (msg.GetType(buf) == MessageType.Disconnect) break;
                    Array.Clear(buf, 0, bytesread);
                }
            }
            catch (System.Net.Sockets.SocketException) {
                view.ShowUserExitMessage(remoteUserInfo);
            }
            finally {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                clientlist.Remove(this);
            }
        }

        public void ConnectToServer(object? clientlist) {
            socket.Bind(new IPEndPoint(localUserInfo.ip, 0));
            socket.Connect(new IPEndPoint(remoteUserInfo.ip, remotePort));
            view.ShowNewUserMessage(remoteUserInfo);
            socket.Send(msg.CreateUserInfoMessage(localUserInfo));
            ReceiveMessages(clientlist as List<TCPClient>);
        }

        public void StopRunning() {
            isRunning = false;
        }

        public TCPClient(int port, UserInfo serverInfo, UserInfo clientInfo, View view, MessageManager msg) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remotePort = port;
            this.remoteUserInfo = serverInfo;
            this.localUserInfo = clientInfo;
            this.view = view;
            this.msg = msg;
        }

        public TCPClient(Socket socket, UserInfo userInfo, View view, MessageManager msg) {
            this.socket = socket;
            this.remoteUserInfo = userInfo;
            this.view = view;
            this.msg = msg;
        }
    }
}
