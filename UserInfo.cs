using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace lab3 {
    internal class UserInfo {
        public String name { get; private set; }
        public IPAddress ip { get; private set; }
        public Color color { get; private set; }
        public UserInfo(String name, IPAddress ip) {
            this.name = name;
            this.ip = ip;
          //  this.color = color;
        }
        public UserInfo(String name, Color color) {
            this.name = name;
            this.color = color;
        }
        public UserInfo(String name, IPAddress ip, Color color) {
            this.name = name;
            this.ip = ip;
            this.color = color;
        }
        public void SetIP(IPAddress ip) {
            this.ip = ip;
        }
        public void SetColor(Color color) {
            this.color = color;
        }
    }
}
