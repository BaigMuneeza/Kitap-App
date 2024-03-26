using KitapRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.SerializationMananger;

namespace KitapCacheManager
{
    public class CacheManager
    {
        string filePath = @"C:\DataFiles";
        string Model;
        string filePath_signup;
        string filePath_pendingAccounts;
        private Dictionary<string, object> _cacheFiles;


        public Dictionary<string, object> CacheFiles
        {
            get { return _cacheFiles; }
            private set { _cacheFiles = value; }
        }

        public Dictionary<string, object> GetCacheData(int enumKey)
        {
            try
            {
                _cacheFiles = new Dictionary<string, object>();

                switch (enumKey)
                {
                    case (int)CacheData.Customer:

                        filePath_signup = Path.Combine(filePath, "CustomerInformation.xml");
                        filePath_pendingAccounts = Path.Combine(filePath, "PendingApprovalSignupInformation.xml");
                        List<CustomerModel> customers = new List<CustomerModel>();
                        List<CustomerModel> pending = new List<CustomerModel>();

                        if (File.Exists(filePath_signup))
                        {
                            customers = SerializationBase.DeserializeData<CustomerModel>(filePath_signup);
                            pending = SerializationBase.DeserializeData<CustomerModel>(filePath_pendingAccounts);
                            _cacheFiles.Add("CustomerModel", customers);
                            _cacheFiles.Add("Pending", pending);
                        }

                        return _cacheFiles;

                    case (int)CacheData.Product:

                        filePath_signup = Path.Combine(filePath, "ProductInformation.save");
                        List<ProductModel> products = new List<ProductModel>();

                        if (File.Exists(filePath_signup))
                        {
                            products = SerializationBase.DeserializeData<ProductModel>(filePath_signup);
                            _cacheFiles.Add("ProductModel", products);
                        }

                        return _cacheFiles;


                    default:
                        Console.WriteLine("incorrect key");
                        break;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return _cacheFiles;
        }
    }
}
