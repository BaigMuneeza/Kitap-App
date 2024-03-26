//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using KitapRepositories;
//using System.Threading;
//using KitapCacheManager;
//using KitapFileManager;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Collections.ObjectModel;


//namespace KitapServer
//{
//    public class servvker
//    {
//            private TcpListener server;
//            Int32 port = 13000;
//            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
//            private const int bufferSize = 1024;
//            private const int heartbeatInterval = 5000;
//            private const int disconnectTimeout = 50000;
//            private Dictionary<string, Client> clientStatusDictionary = new Dictionary<string, Client>();
//            private readonly object _lock = new object();
//            CacheManager cacheManager = new CacheManager();
//            FileManagerServer fileManager = new FileManagerServer();
//            private string heartbeatMessage = "ServerHeartBeat";



//            public servvker()
//            {
//                server = new TcpListener(localAddr, port);
//                server.Start();

//                Task.Run(() => ListenForClientData());
//                Console.WriteLine("Server has started. Waiting for clients...");
//            }


//            public async void ServerStartup()
//            {
//                while (true)
//                {
//                    try
//                    {
//                        TcpClient client = await server.AcceptTcpClientAsync();
//                        HandleClientMethod(client);
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("An error occurred: " + ex.Message);
//                    }
//                }
//            }

//            private async void HandleClientMethod(TcpClient client)
//            {
//                string clientId = client.Client.RemoteEndPoint.ToString();
//                Client clientModel = new Client(clientId, client);
//                Console.WriteLine($"Client connected: {clientId}");

//                lock (_lock)
//                {
//                    if (!clientStatusDictionary.ContainsKey(clientId))
//                    {
//                        clientStatusDictionary.Add(clientId, clientModel);
//                    }
//                }

//                StartHeartbeatTimer(clientModel);
//                SendCacheData(clientModel, 1);

//            }

//            private async void ListenForClientData(/*Client clientModel*/)
//            {
//                if (clientStatusDictionary == null || clientStatusDictionary.Count == 0)
//                    return;

//                try
//                {
//                    while (true)
//                    {
//                        if (clientStatusDictionary.ContainsKey(clientModel.ClientId))
//                        {
//                            NetworkStream stream = clientModel.TcpClient.GetStream();
//                            BinaryFormatter binaryFormatter = new BinaryFormatter();
//                            object receivedData = binaryFormatter.Deserialize(stream);
//                            Type receivedType = receivedData.GetType();

//                            switch (receivedType.Name)
//                            {
//                                case "CustomerModel":

//                                    CustomerModel customerData = (CustomerModel)receivedData;
//                                    bool Ack = fileManager.ProcessReceivedData(4, receivedData);
//                                    break;

//                                case "String":

//                                    string stringData = (string)receivedData;
//                                    if (stringData == "ClientHeartBeat")
//                                    {
//                                        bool Acknowledgement = SendHeartBeat(clientModel, heartbeatMessage);
//                                        clientModel.LastHeartbeatTime = DateTime.Now;
//                                    }
//                                    else if (stringData.Contains("LoggedIn"))
//                                    {
//                                        SendCacheData(clientModel, 2);
//                                    }

//                                    else
//                                    {
//                                        Console.WriteLine("in strig still");
//                                        Console.WriteLine(receivedData);
//                                    }
//                                    break;

//                                case "ObservableCollection`1":

//                                    bool Ack2 = fileManager.ProcessReceivedData(5, receivedData);
//                                    updateClients();
//                                    break;

//                                default:
//                                    break;
//                            }
//                        }

//                        else
//                        {
//                            continue;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error receiving data from client");
//                }
//            }

//            private void updateClients()
//            {
//                lock (_lock)
//                {
//                    foreach (var clientModel in clientStatusDictionary.Values)
//                    {
//                        SendCacheData(clientModel, 2);
//                    }
//                }
//            }

//            private void StartHeartbeatTimer(Client clientModel)
//            {
//                Timer timer = new Timer(state =>
//                {
//                    lock (_lock)
//                    {
//                        if (clientStatusDictionary.ContainsKey(clientModel.ClientId))
//                        {
//                            if ((DateTime.Now - clientModel.LastHeartbeatTime).TotalMilliseconds > disconnectTimeout)
//                            {
//                                clientStatusDictionary.Remove(clientModel.ClientId);
//                                Console.WriteLine($"Client disconnected: {clientModel.ClientId}");
//                                (state as Timer)?.Dispose();
//                            }
//                        }
//                        else
//                        {
//                            (state as Timer)?.Dispose();
//                        }
//                    }
//                }, null, heartbeatInterval, heartbeatInterval);
//            }

//            private bool SendHeartBeat(Client clientModel, string heartbeat)
//            {
//                try
//                {
//                    NetworkStream ns = clientModel.TcpClient.GetStream();
//                    BinaryFormatter binaryFormatter = new BinaryFormatter();
//                    binaryFormatter.Serialize(ns, heartbeat);
//                    ns.Flush();
//                    Console.WriteLine($"Heartbeat sent to client.{clientModel.ClientId}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Client has diconnected");
//                }
//                return false;
//            }



//            private void SendCacheData(Client clientModel, int EnumKey)
//            {
//                try
//                {
//                    NetworkStream stream = clientModel.TcpClient.GetStream();
//                    BinaryFormatter binaryFormatter = new BinaryFormatter();
//                    Dictionary<string, object> cachedData = cacheManager.GetCacheData(EnumKey);
//                    binaryFormatter.Serialize(stream, cachedData);
//                    Console.WriteLine(">> Cache Data sent to client.");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.ToString());
//                }
//            }

//        }
//    }
