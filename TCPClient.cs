using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using P2P_Chat;

namespace lab3 {
    internal class TCPClient {
        Socket socket;
        int serverPort;
        UserInfo server;
        UserInfo client;
        View view;

        public void SendMessage(object? msg) {
            socket.Send(Encoding.Default.GetBytes(msg as String));
        }

        public void ReceiveMessages(object? clientlist) {
            int bytesread = 0;
            socket.Bind(new IPEndPoint(client.ip, 0));
            socket.Connect(new IPEndPoint(server.ip, serverPort));
            view.ShowNewUserMessage(server);
            socket.Send(Encoding.Default.GetBytes(client.name));
            byte[] buf = new byte[1024];
            try {
                while ((bytesread = socket.Receive(buf, 0)) > 0) {
                    view.ShowMessage(server, Encoding.Default.GetString(buf, 0, bytesread));
                    Array.Clear(buf, 0, bytesread);
                }
            }
            catch (System.Net.Sockets.SocketException) { }
            finally {
                view.ShowUserExitMessage(server);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                (clientlist as List<TCPClient>).Remove(this);
            }
        }

        public TCPClient(int port, UserInfo server, UserInfo client, View view) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPort = port;
            this.server = server;
            this.client = client;
            this.view = view;
        }
    }
}
