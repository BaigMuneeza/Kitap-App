using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KitapRepositories
{
    public class Client
    {
        public string ClientId { get; }
        public TcpClient TcpClient { get; }
        public bool IsConnected { get; set; }
        public DateTime LastHeartbeatTime { get; set; }
        public Client(string id, TcpClient tcpClient)
        {
            ClientId = id;
            TcpClient = tcpClient;
            IsConnected = true;
            LastHeartbeatTime = DateTime.Now;
        }

    }
}
