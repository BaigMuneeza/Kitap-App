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
    public partial class DashboardView : Page
    {
        public ObservableCollection<string> Genres { get; set; }
        private BookDisplayVM viewModel;
        private ProductDisplay productView;

        public DashboardView()
        {
             
            InitializeComponent();
            DataContext = this;
            LoadGenres();
            viewModel = new BookDisplayVM();
            productView = new ProductDisplay();
            NavigateToPage(productView);
        }

        private void LoadGenres()
        {
            Genres = new ObservableCollection<string>(GUIHandler.Instance.CacheManager.GetCachedData<List<string>>("Genres"));
            genreListView.ItemsSource = Genres;
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is string selectedGenre)
            {
                string genreName = selectedGenre;
                productView.LoadData(genreName);
            }
        }

        private void NavigateToPage(ProductDisplay bookView)
        {
            navframe2.Navigate(bookView);
        }
    }
}
