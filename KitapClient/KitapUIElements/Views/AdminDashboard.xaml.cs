using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Docking.Helpers;

namespace KitapUIElements.Views
{
    public partial class AdminDashboard : ThemedWindow
    {
        private ManageProductsView _manageProductsView;
        private ManageCustomersView _manageCustomersView;
        private DocumentPanel ManageProducts;
        private DocumentPanel ManageCustomers;
        private DockManager DockManager;
        public AdminDashboard()
        {
            InitializeComponent();
            DockManager = new DockManager();
            _manageProductsView = new ManageProductsView();
            _manageCustomersView = new ManageCustomersView();
            ManageProducts = new DocumentPanel();
            ManageCustomers = new DocumentPanel();
        }

        private void ManageProducts_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            

            if (ManageProducts.Content != null && !ManageProducts.IsClosed)
            {   
                ManageProducts.Focus();
                return;
            }

            ManageProducts = dockLayoutManager1.DockController.AddDocumentPanel(DocumentGroup1);
            ManageProducts.Content = _manageProductsView;
            ManageProducts.Caption = "Products";
            ManageProducts.AllowClose = true;

        }

       

        private void ApproveRequests_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (ManageCustomers.Content != null && !ManageCustomers.IsClosed)
            {
                ManageCustomers.Focus();
                return;
            }

            ManageCustomers = dockLayoutManager1.DockController.AddDocumentPanel(DocumentGroup1);
            ManageCustomers.Content = _manageCustomersView;
            ManageCustomers.Caption = "Customers";
            ManageCustomers.AllowClose = true;


        }

        private void DataWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
