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
    public partial class OrderConfirmed : Page
    {
        public OrderConfirmed()
        {
            InitializeComponent();
        }
        private void GoToDashboard(object sender, RoutedEventArgs e)
        {
            Window mainAppWindow = GUIHandler.Instance.CurrentWindow;
            GUIHandler.Instance.SetView<DashboardView>(mainAppWindow);
        }
    }
}
