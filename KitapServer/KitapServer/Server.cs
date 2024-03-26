using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KitapRepositories;
using System.Threading;
using KitapFileManager;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;

namespace KitapServer
{
    public class Server
    {
        private TcpListener server;
        Int32 port = 13000;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        private const int bufferSize = 1024;
        private const int heartbeatInterval = 5000;
        private const int disconnectTimeout = 50000;
        private Dictionary<string, Client> clientStatusDictionary = new Dictionary<string, Client>();
        private readonly object _lock = new object();
        FileManagerServer fileManager = new FileManagerServer();
        private string heartbeatMessage = "ServerHeartBeat";

        public Server()
        {
            server = new TcpListener(localAddr, port);
            server.Start();
            CacheManagerServer.Instance.loadCache();
            Console.WriteLine("Server has started. Waiting for clients...");
            Console.CancelKeyPress += Console_CancelKeyPress;
        }


        public async void ServerStartup()
        {
            while (true)
            {
                try
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    HandleClientMethod(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        private async void HandleClientMethod(TcpClient client)
        {
            string clientId = client.Client.RemoteEndPoint.ToString();
            Client clientModel = new Client(clientId, client);
            Console.WriteLine($"Client connected: {clientId}");

            lock (_lock)
            {
                if (!clientStatusDictionary.ContainsKey(clientId))
                {
                    clientStatusDictionary.Add(clientId, clientModel);
                }
            }
            Task.Run(() => ListenForClientData(clientModel));
            StartHeartbeatTimer(clientModel);
            SendCacheData(clientModel, 1);

        }

        private async void ListenForClientData(Client clientModel)
        {

            try
            {
                while (true)
                {
                    if (clientStatusDictionary.Count > 0 && clientStatusDictionary.ContainsKey(clientModel.ClientId))
                    {
                        NetworkStream stream = clientModel.TcpClient.GetStream();
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        object receivedData = binaryFormatter.Deserialize(stream);
                        Type receivedType = receivedData.GetType();

                        switch (receivedType.Name)
                        {
                            case "String":

                                string stringData = (string)receivedData;
                                if (stringData == "ClientHeartBeat")
                                {
                                    bool Acknowledgement = SendHeartBeat(clientModel, heartbeatMessage);
                                    clientModel.LastHeartbeatTime = DateTime.Now;
                                }
                                else if (stringData.Contains("LoggedIn"))
                                {
                                    SendCacheData(clientModel, 2);
                                }
                                else if (stringData.Contains("ShutDown"))
                                {
                                    HandleClientShutDown(clientModel);
                                }

                                else
                                {
                                    Console.WriteLine(receivedData);
                                }
                                break;

                            case "CustomerModel":
                                CustomerModel customerData = (CustomerModel)receivedData;

                                if (CacheManagerServer.Instance.InPending(customerData))
                                {
                                    fileManager.ProcessReceivedData(1, receivedData);
                                }
                                else
                                {
                                    bool Ack = fileManager.ProcessReceivedData(2, receivedData);
                                    Console.WriteLine("Recieved pending request");
                                }
                                updateClients(1);

                                break;

                            case "ObservableCollection`1":
                                Type genericType = receivedType.GetGenericArguments()[0];
                                if (genericType == typeof(CartElementModel))
                                {
                                    bool Ack2 = fileManager.ProcessReceivedData(3, receivedData);
                                }

                                else if (genericType == typeof(ProductModel))
                                {
                                    bool Ack2 = fileManager.ProcessReceivedData(4, receivedData);
                                }

                                updateClients(3);
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving data from client");
            }
        }

        private void HandleClientShutDown(Client clientModel)
        {
            lock (_lock)
            {
                if (clientStatusDictionary.ContainsKey(clientModel.ClientId))
                {
                    clientStatusDictionary.Remove(clientModel.ClientId);
                    Console.WriteLine($"Client {clientModel.ClientId} has been removed from the dictionary.");
                }
            }
        }

        private void updateClients(int key)
        {
            lock (_lock)
            {
                foreach (var clientModel in clientStatusDictionary.Values)
                {
                    SendCacheData(clientModel, key);
                }
            }
        }

        private void StartHeartbeatTimer(Client clientModel)
        {
            Timer timer = new Timer(state =>
            {
                lock (_lock)
                {
                    if (clientStatusDictionary.ContainsKey(clientModel.ClientId))
                    {
                        if ((DateTime.Now - clientModel.LastHeartbeatTime).TotalMilliseconds > disconnectTimeout)
                        {
                            clientStatusDictionary.Remove(clientModel.ClientId);
                            Console.WriteLine($"Client disconnected: {clientModel.ClientId}");
                            (state as Timer)?.Dispose();
                        }
                    }
                    else
                    {
                        (state as Timer)?.Dispose();
                    }
                }
            }, null, heartbeatInterval, heartbeatInterval);
        }

        private bool SendHeartBeat(Client clientModel, string heartbeat)
        {
            try
            {
                NetworkStream ns = clientModel.TcpClient.GetStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ns, heartbeat);
                ns.Flush();
                Console.WriteLine($"Heartbeat sent to client.{clientModel.ClientId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client has diconnected");
            }
            return false;
        }

        private void SendCacheData(Client clientModel, int EnumKey)
        {
            try
            {
                NetworkStream stream = clientModel.TcpClient.GetStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Dictionary<string, object> cachedData = CacheManagerServer.Instance.GetCacheData(EnumKey);
                binaryFormatter.Serialize(stream, cachedData);
                Console.WriteLine(">> Cache Data sent to client.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            server.Stop();
            Console.WriteLine("Server is shutting down...");

            if (clientStatusDictionary != null)
            {
                foreach (var clientModel in clientStatusDictionary.Values)
                {
                    try
                    { 
                        if (clientModel.TcpClient.Connected) clientModel.TcpClient.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error closing connection for client {clientModel.ClientId}: {ex.Message}");
                    }
                }
                clientStatusDictionary.Clear();
            }

            CacheManagerServer.Instance.OnServerClose();
            Environment.Exit(0);
        }

    }
}
