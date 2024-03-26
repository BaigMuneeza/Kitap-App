using System;
using KitapUIElements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Net.Sockets;
using KitapClientMananger;
using KitapUIElements.Views;
using System.Net.NetworkInformation;
using System.Windows;
using KitapCache;
using System.Runtime.Serialization.Formatters.Binary;
using KitapRepositories;

namespace KitapApp
{
    public class Connection
    {
        private bool connected = false;

        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        public void ConnectionInit()
        {
            GUIHandler.Instance.ClientManager = ClientManager.Instance;
            GUIHandler.Instance.CacheManager = CacheManager.Instance;
            GUIHandler.Instance.ClientManager.onConnected += OnServerConnect;       
            GUIHandler.Instance.ClientManager.onUnavailable += OnServerUnavailable;
            GUIHandler.Instance.ClientManager.onRecieve += OnRecieve;

            if (!GUIHandler.Instance.ClientManager.Connected)
            {               
                GUIHandler.Instance.ClientManager.ConnectToServer();
            }
        }

        private void OnServerConnect()
        {
            Task.Run(() => GUIHandler.Instance.ClientManager.SendHeartbeat(GetConnectionStatus));
        }

        private void OnRecieve(NetworkStream stream, byte[] buffer)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object receivedData = binaryFormatter.Deserialize(stream);
            Type receivedType = receivedData.GetType();

            try 
            {
                if (receivedType == typeof(String) && receivedData.ToString() == "ServerHeartBeat")
                {
                    GUIHandler.Instance.ClientManager.Connected = true;
                    Console.WriteLine($"Heartbeat recieveed from server.");
                }
                else if(receivedType == typeof(Dictionary<string, object>))
                {
                    var receivedDictionary = (Dictionary<string, object>)receivedData;
                    int keyCount = receivedDictionary.Count;

                    if (GUIHandler.Instance.CacheManager.ReceivedData == null)
                    {
                        GUIHandler.Instance.CacheManager.ReceivedData = (Dictionary<string, object>)receivedData;
                    }

                    else if (GUIHandler.Instance.CacheManager.ReceivedData != null && keyCount==2 && receivedDictionary.ContainsKey("CustomerModel"))
                    {
                        GUIHandler.Instance.CacheManager.UpdateCustomerCache("CustomerModel", (Dictionary<string, object>)receivedData);
                        GUIHandler.Instance.CacheManager.UpdateCustomerCache("Pending", (Dictionary<string, object>)receivedData);
                    }
                    else if (GUIHandler.Instance.CacheManager.ReceivedData != null && keyCount == 2 && receivedDictionary.ContainsKey("Genres"))
                    {
                        GUIHandler.Instance.CacheManager.AppendCache("ProductModel", (Dictionary<string, object>)receivedData);
                        GUIHandler.Instance.CacheManager.AppendCache("Genres", (Dictionary<string, object>)receivedData);
                    }
                    else
                    {
                        
                        GUIHandler.Instance.CacheManager.AppendCache("ProductModel", (Dictionary<string, object>)receivedData);
                    }

                    GUIHandler.Instance.CacheManager.IsConnected = true;
                }
                else
                {
                    Console.WriteLine(receivedData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private void OnServerUnavailable()
        {
            Console.WriteLine("OnServerUnavailable");

            if (!GUIHandler.Instance.CacheManager.IsConnected)
            {
                if (MessageBox.Show("Server is Down! Do you want to try again?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    GUIHandler.Instance.ClientManager.ConnectToServer();
                }
                else
                {
                    GUIHandler.Instance.ClientManager.DisconnectFromServer();
                }
            }
            else
            {
                if (MessageBox.Show("Server is not responding! Do you want to try again?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    GUIHandler.Instance.ClientManager.ConnectToServer();
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Application.Current.Shutdown();
                    });
                    GUIHandler.Instance.ClientManager.DisconnectFromServer();
                }
            }
        }

        private void GetConnectionStatus(bool status)
        {
            Connected = status;
        }
    }
}
