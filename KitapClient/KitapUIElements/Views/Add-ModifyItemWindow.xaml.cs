using DevExpress.Xpf.Core;
using KitapRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace KitapUIElements.Views
{
    public partial class Add_ModifyItemWindow : ThemedWindow
    {
        private ProductModel productModel;
        public Add_ModifyItemWindow()
        {
            InitializeComponent();
            productModel = new ProductModel();
            btnModifyProduct.Visibility = Visibility.Collapsed;

        }

        public void LoadSelectedRow(ProductModel selectedrow)
        {
            btnAddProduct.Visibility = Visibility.Collapsed;
            btnModifyProduct.Visibility = Visibility.Visible;
            AddModifyHearder.Content = "Modify Product";
            productModel = selectedrow as ProductModel;
            if (productModel != null)
            {
                txtGenre.Text = productModel.BookGenre;
                txtTitle.Text = productModel.BookTitle;
                txtAuthor.Text = productModel.Author ;
                txtImage.Text = productModel.CoverImage;
                txtStock.Text = Convert.ToString(productModel.ProductStock);
            }
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (productModel != null && !string.IsNullOrEmpty(txtGenre.Text) && !string.IsNullOrEmpty(txtTitle.Text) && !string.IsNullOrEmpty(txtAuthor.Text) && !string.IsNullOrEmpty(txtImage.Text) && !string.IsNullOrEmpty(txtStock.Text))
            {
                productModel.BookId = (GUIHandler.Instance.CacheManager.GetCachedData<List<ProductModel>>("ProductModel").Last().BookId) + 1;
                productModel.BookGenre = txtGenre.Text;
                productModel.BookTitle = txtTitle.Text;
                productModel.Author = txtAuthor.Text;
                productModel.CoverImage = txtImage.Text;
                productModel.ProductStock = int.Parse(txtStock.Text);
                GUIHandler.Instance.CacheManager.AddObjectToCache("ProductModel", productModel);
                Close();
            }
        }

        private void BtnModifyProduct_Click(object sender, RoutedEventArgs e)
        {
            if (productModel != null && !string.IsNullOrEmpty(txtGenre.Text) && !string.IsNullOrEmpty(txtTitle.Text) && !string.IsNullOrEmpty(txtAuthor.Text) && !string.IsNullOrEmpty(txtImage.Text) && !string.IsNullOrEmpty(txtStock.Text))
            {
                productModel.BookGenre = txtGenre.Text;
                productModel.BookTitle = txtTitle.Text;
                productModel.Author = txtAuthor.Text;
                productModel.CoverImage = txtImage.Text;
                productModel.ProductStock = int.Parse(txtStock.Text);
                GUIHandler.Instance.CacheManager.CacheModifyProduct(productModel);
                Close();
            }       
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
