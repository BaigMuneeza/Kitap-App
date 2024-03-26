using KitapRepositories;
using KitapUIElements.Utilities;
using KitapUIElements.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KitapUIElements.ViewModels
{
    public class CartVM : ViewModelBase
    {
        private List<CartElementModel> _cartHistory {  get; set; }
        private ObservableCollection<CartElementModel> _cart;
        public ICommand RemoveFromCartCommand { get; }


        public ObservableCollection<CartElementModel> CartCollection
        {
            get { return _cart; }
            set { _cart = value; }
        }

        public List<CartElementModel> CartHistory
        {
            get { return _cartHistory; }
            set { _cartHistory = value; }
        }

        public CartVM()
        {
            _cartHistory = new List<CartElementModel>();
            CartCollection = new ObservableCollection<CartElementModel>();
            RemoveFromCartCommand = new RelayCommand(ExecuteRemoveFromCartCommand);
        }

        internal void SetCart(ObservableCollection<CartElementModel> cart)
        {
            CartCollection.Clear();
            foreach (var book in cart)
            {
                if (book is CartElementModel selectedBook)
                {
                    CartElementModel searchedbook = _cartHistory.Find(x => x.BookId == selectedBook.BookId);

                    if (searchedbook != null)
                    {
                        searchedbook.Quantity = selectedBook.Quantity;
                    }

                    else
                    {
                        _cartHistory.Add(selectedBook);
                    }
                }
            }

            UpdateCartCollection(_cartHistory);
        }

        private void ExecuteRemoveFromCartCommand(object obj)
        {
            if (obj is CartElementModel selectedBook)
            {
                CartCollection.Remove(selectedBook);
                _cartHistory.Remove(selectedBook);   
            }
        }

        private void UpdateCartCollection(List<CartElementModel> carthistory)
        {
            CartCollection.Clear();
            foreach (var item in carthistory)
            {
                CartCollection.Add(item);
            }
        }
    }
}
