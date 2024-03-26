using KitapServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KitapServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.ServerStartup();
            Console.ReadKey();
        }
    }
}
