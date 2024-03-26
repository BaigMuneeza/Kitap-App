using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KitapApp
{
    public class KitapApp 
    {
        private Connection connection;

        public KitapApp() { }

        public void Init()
        {
            connection = new Connection();
            if (!connection.Connected) connection.ConnectionInit();
        }
    }
}
