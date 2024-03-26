using KitapRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.SerializationMananger;

namespace Utilities.CacheManangerServer
{
    public class CacheManager
    {
        string filePath_signup;
        private Dictionary<string, object> _cacheFiles;


        public Dictionary<string, object> CacheFiles
        {
            get { return _cacheFiles; }
            private set { _cacheFiles = value; }
        }

        public Dictionary<string, object> ReadCacheDataFromFile()
        {
            filePath_signup = "CustomerInformation.save";
            List<CustomerModel> customers = new List<CustomerModel>();

            
            if (File.Exists(filePath_signup))
            {

                customers = SerializationBase.DeserializeData<CustomerModel>(filePath_signup);
                _cacheFiles = new Dictionary<string, object>();
                _cacheFiles.Add("CustomerModel", customers);
            }
            else
            {
                _cacheFiles = new Dictionary<string, object>();
            }

            return _cacheFiles;
        }
    }
}
