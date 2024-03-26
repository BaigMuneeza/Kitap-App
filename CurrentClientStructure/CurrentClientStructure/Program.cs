using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appclientconsole
{
    public partial class Program
    {
            [STAThread()]
            static void Main()
            {
                KitapApp app = new KitapApp();
                app.initAsync();
                Console.ReadKey();
        }


       
    }
}


