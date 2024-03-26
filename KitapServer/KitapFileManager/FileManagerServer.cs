using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitapRepositories;
using Utilities.SerializationMananger;


namespace KitapFileManager
{
    public class FileManagerServer
    {
        public bool ProcessReceivedData(int enumKey, object receivedData)
        {
            try
            {
                switch (enumKey)
                {
                    case (int)DataReceived.ApproveRequest:
                        ProcessSignUpData(receivedData, 1);
                        break;

                    case (int)DataReceived.PendingRequest:
                        ProcessSignUpData(receivedData, 0);
                        break;

                    case (int)DataReceived.AdjustQuantity:
                        AdjustingStocks(receivedData);
                        break;

                    case (int)DataReceived.UpdateProducts:
                        UpdateProducts(receivedData);
                        break;

                    default:
                        Console.WriteLine("Incorrect key");
                        break;
                }
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        private void UpdateProducts(object receivedData)
        {
            ObservableCollection<ProductModel> productsInformation = (ObservableCollection<ProductModel>)receivedData;
            CacheManagerServer.Instance.UpdateCachedData("ProductModel", productsInformation);
        }

        public void ProcessSignUpData(object receivedObject, int enumKey)
        {            

            try
            {
                switch (enumKey)
                {
                    case (int)ApprovalStatus.Pending:

                        bool addedSuccessfully = CacheManagerServer.Instance.AddObjectToCache("Pending", (CustomerModel)receivedObject);
                        if (addedSuccessfully)
                        {
                            Console.WriteLine(">> New pending request!");
                        }
                        break;

                    case (int)ApprovalStatus.Approved:

                        bool removedsuccesfully = CacheManagerServer.Instance.RemoveObjectFromCache("Pending", (CustomerModel)receivedObject);
                        bool approvedSuccessfully = CacheManagerServer.Instance.AddObjectToCache("CustomerModel", (CustomerModel)receivedObject);
                        
                        if (approvedSuccessfully)
                        {
                            Console.WriteLine("A pending request was approved!");
                        }       
                        break;

                    default:
                        Console.WriteLine("Incorrect key");
                        break;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void AdjustingStocks(object receivedData)
        {
            ObservableCollection<CartElementModel> cartData = (ObservableCollection<CartElementModel>)receivedData;
            foreach (CartElementModel cartItem in cartData)
            {
                ProductModel product = CacheManagerServer.Instance.GetCachedData<List<ProductModel>>("ProductModel").Find(x => x.BookId == cartItem.BookId);
                if (product != null)
                {
                    product.ProductStock -= cartItem.Quantity;
                }
            }    
            Console.WriteLine(">> Stocks Updated");
        }

    }
}