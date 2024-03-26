using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KitapUIElements.Views
{
    public partial class CancelMenu : UserControl
    {
        public CancelMenu()
        {
            InitializeComponent();
        }
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        { 
            Window window = GUIHandler.Instance.CurrentWindow;
            window.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close the application?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GUIHandler.Instance.ClientManager.SendAckToServer("ShutDown");
                GUIHandler.Instance.ClientManager.DisconnectFromServer();
                Application.Current.Shutdown();         
            }
        }
    }
}
