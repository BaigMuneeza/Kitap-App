using KitapRepositories;
using KitapUIElements.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KitapUIElements.ViewModels
{
    public class BookDisplayVM : ViewModelBase
    {
        private ObservableCollection<CartElementModel> _books;
        private ObservableCollection<CartElementModel> _cart;
        private List<ProductModel> AllBooks;
        private string _genre;

        public ObservableCollection<CartElementModel> Books 
        { 
            get { return _books; }
            set { _books = value; }
        }
        public ObservableCollection<CartElementModel> Cart
        {
            get { return _cart; }
            set { _cart = value; }
        }

        public ICommand AddToCartCommand { get; }

        public BookDisplayVM()
        {
            GUIHandler.Instance.CacheManager._onUserDashboard = true;
            GUIHandler.Instance.CacheManager.OnCacheUpdated += UpdateBooksCollection;
            AllBooks = GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel");
            Books = new ObservableCollection<CartElementModel>();
            Cart = new ObservableCollection<CartElementModel>();            
        }

        private void UpdateBooksCollection()
        {
           if (_genre != null)
            {
                AllBooks = GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    foreach (var book in AllBooks)
                    {
                        if (book.BookGenre == _genre)
                        {
                            CartElementModel displayBook = new CartElementModel();
                            displayBook.BookId = book.BookId;
                            displayBook.BookGenre = book.BookGenre;
                            displayBook.BookTitle = book.BookTitle;
                            displayBook.Author = book.Author;
                            displayBook.CoverImage = book.CoverImage;
                            displayBook.Quantity = 0;
                            Books.Add(displayBook);
                        }
                    }
                });
            }
        }

        public void AddToCart(object obj)
        {
            if (!(obj is CartElementModel selectedBook)) return;

            CartElementModel book = Cart.FirstOrDefault(x => x.BookId == selectedBook.BookId);

            if (book != null)
            {
                book.Quantity = selectedBook.Quantity;
                MessageBox.Show("Product quanity updated!");
            }

            else
            {
                Cart.Add(selectedBook);
                MessageBox.Show("Product added to cart!");
            }
        }


        public void UpdateBooks(string genre)
        {
            AllBooks = GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel");
            _genre = genre;
            Books.Clear();
            foreach (var book in AllBooks)
            {
                if (book.BookGenre == genre)
                {
                    CartElementModel displayBook = new CartElementModel();
                    displayBook.BookId = book.BookId;
                    displayBook.BookGenre = book.BookGenre;
                    displayBook.BookTitle = book.BookTitle;
                    displayBook.Author = book.Author;
                    displayBook.CoverImage = book.CoverImage;
                    displayBook.Quantity = 0;
                    Books.Add(displayBook);
                }
            }
        }
    }
}

