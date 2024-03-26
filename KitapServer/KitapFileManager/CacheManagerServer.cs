using KitapRepositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.SerializationMananger;

namespace KitapFileManager
{
    public class CacheManagerServer
    {
        private static CacheManagerServer _instance;
        private Dictionary<string, object> cacheLog;
        private const string FilePath = @"C:\DataFiles";
        
        string filePath_Signup = Path.Combine(FilePath, "CustomerInformation.xml");
        string filePath_PendingSignup = Path.Combine(FilePath, "PendingApprovalSignupInformation.xml");
        string filePath_Product = Path.Combine(FilePath, "ProductInformation.xml");
        string filePath_Genres= Path.Combine(FilePath, "Genres.save");

        List<CustomerModel> customers = new List<CustomerModel>();
        List<CustomerModel> pendingApprovals = new List<CustomerModel>();
        List<string> genres = new List<string>();
        private List<ProductModel> products = new List<ProductModel>();
        Dictionary<string, object> cacheData;

        public void loadCache()
        {
            if (File.Exists(filePath_Signup) && File.Exists(filePath_PendingSignup) && File.Exists(filePath_Product))
            {
                customers = SerializationBase.DeserializeData<CustomerModel>(filePath_Signup);
                pendingApprovals = SerializationBase.DeserializeData<CustomerModel>(filePath_PendingSignup);
                products = SerializationBase.DeserializeData<ProductModel>(filePath_Product);
                genres = SerializationBase.DeserializeData<string>(filePath_Genres);

                cacheLog.Add("CustomerModel", customers);
                cacheLog.Add("Pending", pendingApprovals);
                cacheLog.Add("ProductModel", products);
                cacheLog.Add("Genres", genres);
                Console.WriteLine("Cache loaded");
            }
        }

        private CacheManagerServer()
        {
            cacheLog = new Dictionary<string, object>();
        }
        public static CacheManagerServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CacheManagerServer();
                }
                return _instance;
            }
        }
        public Dictionary<string, object> CacheFiles
        {
            get { return cacheLog; }
            private set { cacheLog = value; }
        }

        public T GetCachedData<T>(string key) where T : class
        {
            if (cacheLog.ContainsKey(key))

                return cacheLog[key] as T;

            return null;
        }

        public bool AddObjectToCache(string key, object value)
        {
            if (cacheLog.ContainsKey(key))
            {
                if (cacheLog[key] is IList list)
                {
                    list.Add(value);
                    return true;
                }
            }
            return false;
        }
        public bool RemoveObjectFromCache(string key, CustomerModel value)
        {
            if (cacheLog.ContainsKey(key))
            {
                if (cacheLog[key] is IList list)
                {
                    var itemToRemove = list.OfType<CustomerModel>().FirstOrDefault(item => item.Username == value.Username);
                    if (itemToRemove != null)
                    {
                        list.Remove(itemToRemove);
                        return true;
                    }
                }
            }
            return false;
        }    

        public void UpdateCachedData<T>(string key, IEnumerable<T> newData)
        {
            if (cacheLog.ContainsKey(key))
            {
                if (cacheLog[key] is IList list)
                {
                    list.Clear();
                    foreach (var item in newData)
                    {
                        list.Add(item);
                    }
                }  
            }
        }
        public Dictionary<string, object> GetCacheData(int enumKey)
        {
            try
            {
                cacheData = new Dictionary<string, object>();

                switch (enumKey)
                {
                    case (int)CacheData.Customer:

                        var customers = CacheManagerServer.Instance.GetCachedData<List<CustomerModel>>("CustomerModel");
                        var pendingApprovals = CacheManagerServer.Instance.GetCachedData<List<CustomerModel>>("Pending");
                        cacheData.Add("CustomerModel", customers);
                        cacheData.Add("Pending", pendingApprovals);
                        return cacheData;


                    case (int)CacheData.Product:

                        var products = CacheManagerServer.Instance.GetCachedData<List<ProductModel>>("ProductModel");
                        var genres = CacheManagerServer.Instance.GetCachedData<List<string>>("Genres");
                        cacheData.Add("ProductModel", products);
                        cacheData.Add("Genres", genres);
                        return cacheData;

                    case (int)CacheData.ProductUpdate:

                        products = CacheManagerServer.Instance.GetCachedData<List<ProductModel>>("ProductModel");
                        cacheData.Add("ProductModel", products);

                        return cacheData;


                    default:
                        Console.WriteLine("Incorrect key");
                        break;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return cacheData;
        }

        public bool InPending(CustomerModel customerData)
        {
            if (customerData != null)
            {
                var pendingApprovals = GetCachedData<List<CustomerModel>>("Pending");
                if (pendingApprovals != null)
                {

                    foreach (var customer in pendingApprovals)
                    {
                        if (customer.Username == customerData.Username)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void OnServerClose()
        {
            foreach (var key in cacheLog.Keys)
            {
                if (key == "CustomerModel")
                {
                    customers = (List<CustomerModel>)cacheLog[key];
                    SerializationBase.SerializeData(filePath_Signup, customers);
                }

                else if (key == "Pending")
                {
                    pendingApprovals = (List<CustomerModel>)cacheLog[key];
                    SerializationBase.SerializeData(filePath_PendingSignup, pendingApprovals);
                }

                else if (key == "ProductModel")
                {
                    products = (List<ProductModel>)cacheLog[key];
                    SerializationBase.SerializeData(filePath_Product, products);
                }
            }
        }
    }
}
