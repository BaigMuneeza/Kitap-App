using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KitapClientMananger
{
    public delegate void CallBack(bool status);
    public delegate void OnConnectedEventHandler();
    public delegate void OnDisconnectedEventHandler();
    public delegate void OnUnavailableEventHandler();
    public delegate void OnRecieveEventHandler(NetworkStream stream, byte[] buffer);

    public class ClientManager
    {
        private static ClientManager instance;
        private bool m_bConnected = false;
        public TcpClient client;
        private const int heartbeatInterval = 5000;
        private const int heartbeatTimeout = 5000;
        private string heartbeatMessage = "ClientHeartBeat";

        public event OnConnectedEventHandler onConnected;
        public event OnDisconnectedEventHandler onDisconnected;
        public event OnUnavailableEventHandler onUnavailable;
        public event OnRecieveEventHandler onRecieve;

        public static ClientManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientManager();

                return instance;
            }
        }

        public ClientManager()  
        { 
            client = new TcpClient(); 
        }


        public bool Connected
        {
            get { return m_bConnected; }
            set { m_bConnected = value; }
        }

        public TcpClient Client
        {
            get { return client; }
        }

        public void ConnectToServer()
        {
            try
            {
                client.Connect("127.0.0.1", 13000);
                Connected = true;              
                onConnected();

            }
            catch (SocketException)
            { 
                onUnavailable();
            }     
        }

        public void DisconnectFromServer()
        {
            try
            {
                client.Close();               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disconnecting from server: " + ex.Message);
            }
        }
        public async void SendHeartbeat(CallBack GetConnectionStatus)
        {
            if (client.Connected)
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    try
                    {
                        while (Connected)
                        {
                            await Task.Delay(heartbeatInterval);
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            binaryFormatter.Serialize(stream, heartbeatMessage);
                            await stream.FlushAsync();
                            Console.WriteLine($"Heartbeat Sent to server");

                            stream.ReadTimeout = heartbeatTimeout;
                            onRecieve(stream, buffer);
                            GetConnectionStatus(Connected);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending or receiving heartbeat: {ex.Message}");
                        Connected = false;
                        GetConnectionStatus(Connected);
                        onUnavailable();
                    }
                }
            }
        }

        public void SendObjectToServer(object data)
        {
            NetworkStream clientStream = client.GetStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(clientStream, data);
            clientStream.Flush();
        }

        public void SendAckToServer(string message)
        {
            NetworkStream clientStream = client.GetStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(clientStream, message);
            clientStream.Flush();
        }

        public bool TestConnection()
        {
            return client.Connected;
        }
    }
}


