using KitapRepositories;
using KitapUIElements.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class OrderDisplayView : Page
    {
        private readonly CartVM _cartViewModel; 
        private OrderConfirmed _orderConfirmatedView;

        public OrderDisplayView()
        {
            _cartViewModel = new CartVM();
            InitializeComponent();
            this.DataContext = _cartViewModel;
            _orderConfirmatedView = new OrderConfirmed();
        }

        internal void SetCart(ObservableCollection<CartElementModel> cart)
        {
            _cartViewModel.SetCart(cart);
        }

        private void ConfirmOrderView_Click(object sender, RoutedEventArgs e)
        {
            if (_cartViewModel.CartCollection.Count == 0 ) 
            {
                MessageBox.Show("All cart elements have been removed");
            }
            else
            {
                GUIHandler.Instance.ClientManager.SendObjectToServer(_cartViewModel.CartCollection);
                Window mainAppWindow = GUIHandler.Instance.CurrentWindow;
                mainAppWindow.Content = _orderConfirmatedView;
                _cartViewModel.CartCollection.Clear();
                _cartViewModel.CartHistory.Clear();
            }

        }

        private void BackToDash_Click(object sender, RoutedEventArgs e)
        {
            Window mainAppWindow = GUIHandler.Instance.CurrentWindow;
            GUIHandler.Instance.SetView<DashboardView>(mainAppWindow);
        }


    }
}
