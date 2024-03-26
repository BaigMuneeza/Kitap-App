using KitapCacheManager;
using KitapRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.SerializationMananger;

namespace KitapFileManager.DataHandler
{
    public class StockData
    {
        string filePath = @"C:\DataFiles";
        string filePath_signup;
        public void AdjustingStocks(object receivedData)
        {
            ObservableCollection<CartElementModel> cartData = (ObservableCollection<CartElementModel>)receivedData;
            filePath_signup = Path.Combine(filePath, "ProductInformation.save");
            List<ProductModel> products = new List<ProductModel>();

            if (File.Exists(filePath_signup))
            {
                products = SerializationBase.DeserializeData<ProductModel>(filePath_signup);
                foreach (CartElementModel cartItem in cartData)
                {
                    ProductModel product = products.Find(x => x.BookId == cartItem.BookId);
                    if (product != null)
                    {
                        product.ProductStock -= cartItem.Quantity;
                    }
                }
            }
            SerializationBase.SerializeData(filePath_signup, products);
            Console.WriteLine("Stocks Updated");
        }
    }
}
