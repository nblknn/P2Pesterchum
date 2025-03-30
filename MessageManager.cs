using System.Windows.Media;
using System.Text;
using lab3;

namespace P2P_Chat {
    public enum MessageType {
        UserInfo, Text, Disconnect
    };
    internal class MessageManager {
        public byte[] CreateTextMessage(string msg) {
            byte[] message = new byte[Encoding.Default.GetBytes(msg).Length + 1];
            message[0] = (byte)MessageType.Text;
            Array.Copy(Encoding.Default.GetBytes(msg), 0, message, 1, message.Length - 1);
            return message;
        }
        public byte[] CreateUserInfoMessage(UserInfo user) {
            int len = user.name.Length;
            byte[] message = new byte[len + 4];
            message[0] = (byte)MessageType.UserInfo;
            Array.Copy(Encoding.Default.GetBytes(user.name), 0, message, 1, len);
            message[len + 1] = user.color.R;
            message[len + 2] = user.color.G;
            message[len + 3] = user.color.B;
            return message;
        }
        public byte[] CreateDisconnectMessage() {
            byte[] message = new byte[1];
            message[0] = (byte)MessageType.Disconnect;
            return message;
        }
        public MessageType GetType(byte[] message) {
            return (MessageType)message[0];
        }
        public UserInfo GetUserInfo(byte[] message, int len) {
            return new UserInfo(Encoding.Default.GetString(message, 1, len - 4), Color.FromRgb(message[len - 3],
                message[len - 2], message[len - 1]));
        }
        public String GetText(byte[] message, int len) {
            return Encoding.Default.GetString(message, 1, len - 1);
        }
    }
}
