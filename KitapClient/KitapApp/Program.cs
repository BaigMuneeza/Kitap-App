using KitapUIElements;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using KitapUIElements.Views;

namespace KitapApp
{
    public partial class Program
    {
        [STAThread()]
        static void Main()
        {
            KitapApp app = new KitapApp();
            app.Init();

            if (GUIHandler.Instance.ClientManager.Connected)
            {
                System.Windows.Application appMain = new System.Windows.Application();
                MainWindow AppSetUpWindow = new MainWindow();
                appMain.Run(AppSetUpWindow);
            }
        }
    }
}
