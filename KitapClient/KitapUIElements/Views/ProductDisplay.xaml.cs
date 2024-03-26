using KitapRepositories;
using KitapUIElements.ViewModels;
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

    public partial class ProductDisplay : UserControl
    {
        private BookDisplayVM _bookDisplayViewModel;
        private OrderDisplayView _orderView;

        MainAppWindow mainAppWindow = App.Current.MainWindow as MainAppWindow;
        public ProductDisplay()
        {
            _bookDisplayViewModel = new BookDisplayVM();
            this.DataContext = _bookDisplayViewModel;
            InitializeComponent();
            _orderView = new OrderDisplayView();
        }

        public void LoadData(string genre)
        {
            _bookDisplayViewModel.UpdateBooks(genre);      
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            CartElementModel selectedBook = (CartElementModel)button.DataContext;
            var bookInCache = GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel").FirstOrDefault(x => x.BookId == selectedBook.BookId);

            if (selectedBook.Quantity!=0)
            {
                if (selectedBook.Quantity < bookInCache.ProductStock) _bookDisplayViewModel.AddToCart(selectedBook);
                else { MessageBox.Show("Select a smaller quantity");}

            }

            else
            {
                MessageBox.Show("Select a bigger quantity");
            }
            
  
        }
        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_bookDisplayViewModel.Cart.Count == 0) { MessageBox.Show("Cart is empty"); }
            else
            {

                Window mainAppWindow = GUIHandler.Instance.CurrentWindow;
                mainAppWindow.Content = _orderView;
                _bookDisplayViewModel.Books.Clear();
                _orderView.SetCart(_bookDisplayViewModel.Cart);
                _bookDisplayViewModel.Cart.Clear();  
            }
        }
    }
}