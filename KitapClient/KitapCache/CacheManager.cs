using KitapClientMananger;
using KitapRepositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KitapCache
{
    public class CacheManager
    {
        public delegate void OnCacheDataGetEventHandler();
        public event OnCacheDataGetEventHandler OnCacheDataGet;

        public delegate void OnCacheUpdatedEventHandler();
        public event OnCacheUpdatedEventHandler OnCacheUpdated;

        public delegate void OnQuantityUpdatedEventHandler();
        public event OnQuantityUpdatedEventHandler OnQuantityUpdated;

        private static CacheManager _instance;
        private Dictionary<string, object> _receivedData;
        private Dictionary<string, object> _receivedProductInformation;
        private Dictionary<string, object> _receivedInformation;
        private bool _productData;
        public bool _onUserDashboard = false;
        public bool _onAdminDashboard = false;
        public bool IsConnected { get; set; }
        public Dictionary<string, object> ReceivedData
        {
            get { return _receivedData; }
            set { _receivedData = value; }
        }

        public bool HasProductData
        {
            get { return _productData; }
            set { _productData = value; }
        }
        public static CacheManager Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new CacheManager();
                }
                return _instance;
            }
        }

        
        public T GetCachedData<T>(string key) where T : class
        {
            if(_receivedData != null)
            {
                if (_receivedData.ContainsKey(key))

                    return _receivedData[key] as T;
            }
            return null;
        }


        public T GetBooksData<T>(string genre) where T : class
        {
            var genreSpecificList = new List<ProductModel>();
            if (_receivedInformation != null)
            {
                if (_receivedData.ContainsKey("ProductModel"))
                {

                    List<ProductModel> booksList = GetCachedData<List<ProductModel>>("ProductModel");

                    foreach (ProductModel productModel in booksList)
                    {
                        if (productModel.BookGenre == genre)
                        {
                            genreSpecificList.Add(productModel);
                        }
                    }

                }
            } 
            return genreSpecificList as T;
        }
        public bool AddObjectToCache(string key, object value)
        {
            if (_receivedData != null)
            {
                if (_receivedData.ContainsKey(key))
                {
                    if (_receivedData[key] is IList list)
                    {
                        list.Add(value);
                        return true;
                    }
                }
            }

             return false;    
        }

        public void CacheModifyProduct(ProductModel datarow) 
        {
            ProductModel product = CacheManager.Instance.GetCachedData<List<ProductModel>>("ProductModel").Find(x => x.BookId == datarow.BookId);
            if (product != null)
            {
                product.ProductStock = datarow.ProductStock;
                product.BookTitle = datarow.BookTitle;
                product.BookGenre = datarow.BookGenre;
                product.Author = datarow.Author;
                product.CoverImage = datarow.CoverImage;
            }
        }
        public void AppendCache(string key, object recivedDictionary)
        {
            _receivedProductInformation = (Dictionary<string, object>)recivedDictionary;
            if (key == "ProductModel")
            {
                List<ProductModel> booksList = (List<ProductModel>)_receivedProductInformation[key];
                _receivedData[key] = booksList;
                HasProductData = true;

                if (_onUserDashboard)  OnCacheUpdated();
                
                if (_onAdminDashboard) OnQuantityUpdated();
                
            }
            else if (key == "Genres")
            {
                List<string> genreList = (List<string>)_receivedProductInformation[key];
                _receivedData[key] = genreList;
            }

        }
        public void UpdateCustomerCache(string key, object recivedDictionary)
        {
            _receivedInformation = (Dictionary<string, object>)recivedDictionary;
            List<CustomerModel> customerList = (List<CustomerModel>)_receivedInformation[key];
            if (customerList != null)
            {
                _receivedData[key] = customerList;
            }
           

        }
    }
}
