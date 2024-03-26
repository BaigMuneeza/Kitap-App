using DevExpress.Xpf.Grid;
using DevExpress.XtraExport.Helpers;
using KitapRepositories;
using KitapUIElements.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    public partial class ManageProductsView : UserControl
    {
        private ObservableCollection<ProductModel> _products;
        private List<ProductModel> AllProducts;
        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; }
        }

        public ObservableCollection<ProductModel> Selection { get; } = new ObservableCollection<ProductModel>();
        public ManageProductsView()
        {
            DataContext = this;
            GUIHandler.Instance.CacheManager._onAdminDashboard = true;
            GUIHandler.Instance.CacheManager.OnQuantityUpdated += UpdateBooksQuantity;
            Products = new ObservableCollection<ProductModel>(GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel"));
            InitializeComponent();
        }

        private void UpdateBooksQuantity()
        {
            AllProducts = GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel");
            Application.Current.Dispatcher.Invoke(() =>
            {
                Products.Clear();

                foreach (var book in AllProducts)
                {
                    Products.Add(book);
                }
                GridManageProducts.RefreshData();
            });
           
        }

        private void DeleteRow(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this product?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                view.DeleteRow(view.FocusedRowHandle);
                MessageBox.Show("Product deleted");
                GUIHandler.Instance.ClientManager.SendObjectToServer(Products);
            }
        }

        private void AddRow(object sender, RoutedEventArgs e)
        {
            Add_ModifyItemWindow AddWindow = new Add_ModifyItemWindow();
            AddWindow.ShowDialog();
            RefreshItems();
        }

        private void ModifyRow(object sender, RoutedEventArgs e)
        {
            IList selectedRows = Selection.ToList();
            if (selectedRows.Count > 0) {
                ProductModel selectedrow = (ProductModel)selectedRows[0];
                Add_ModifyItemWindow AddWindow = new Add_ModifyItemWindow();
                AddWindow.LoadSelectedRow(selectedrow);
                AddWindow.ShowDialog();
                RefreshItems();
            }
            else
            {
                 MessageBox.Show("No item selected");
            }

        }

        private void RefreshItems()
        {
            Products.Clear();
            AllProducts = GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel");
            foreach (var book in AllProducts)
            {
                Products.Add(book);

            }
            GridManageProducts.RefreshData();
            GUIHandler.Instance.ClientManager.SendObjectToServer(Products);
        }
    }
}
