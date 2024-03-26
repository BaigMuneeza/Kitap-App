using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KitapClientMananger
{
    //delegates
    public delegate void CallBack(bool status);
    //---------------------------------------
    public delegate void ConnectRequest();
    public delegate void DisconnectRequest();
    public delegate void HeartbeatThread(CallBack statusObj);

    public class ClientManager
    {
        private static ClientManager instance;
        private bool connected = false;
        private TcpClient client;
        private const int heartbeatInterval =2000;
        private const int heartbeatTimeout = 5000;
        private string heartbeatMessage = "ClientHeartBeat";


        //define delegate objects
        public CallBack serverCallBack;
        public ConnectRequest connectRequest;
        public DisconnectRequest disconnectRequest;
        public HeartbeatThread heartbeatThread;

        public ClientManager()
        {
            client = new TcpClient();
            if (client.Connected) 
            {
                Connected = true;
            }
        }
        public bool Connected
        {
            get { return connected; }
            set { connected = value;}
        }
        public static ClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientManager();
                }
                return instance;
            }
        }
        public void ConnectToServer()
        {
            client.Connect("127.0.0.1", 13000);
            if (client.Connected)
            {
                Connected = true;
            }
        }

    public void DisconnectFromServer()
        {
            try
            {
                client.Close();
                Console.WriteLine("Disconnected from server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disconnecting from server: " + ex.Message);
            }
        }
        public async void SendHeartbeat(CallBack statusObj)
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
                            //send client heartbeat
                            await Task.Delay(heartbeatInterval);
                            byte[] bytesToSend = Encoding.UTF8.GetBytes(heartbeatMessage);
                            await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
                            await stream.FlushAsync(); // Use async Flush method

                            //receive server heartbeat
                            stream.ReadTimeout = heartbeatTimeout;
                            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            if (receivedData == "ServerHeartBeat")
                            {
                               // serverConnection = true;
                                statusObj(true);
                                continue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending or receiving heartbeat: {ex.Message}");
                       // serverConnection = false;
                        statusObj(false);
                    }
                }
            }
        }
        //public async void SendHeartbeat()
        //{
        //    if (client.Connected) 
        //    { 
        //        NetworkStream stream = client.GetStream();
        //        byte[] buffer = new byte[1024];
        //        while (serverConnection)
        //        {
        //            try
        //            {          

        //                //send client heartbeat
        //                await Task.Delay(heartbeatInterval);                    
        //                byte[] bytesToSend = Encoding.UTF8.GetBytes(heartbeatMessage);
        //                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
        //                stream.Flush();
        //                //Console.WriteLine($"Heartbeat sent to server.");


        //                //recieve server heartbeat
        //                stream.ReadTimeout = heartbeatTimeout;
        //                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //                if (receivedData == "ServerHeartBeat")
        //                {
        //                    //Console.WriteLine($"Heartbeat recieved from client.");
        //                    serverConnection = true;
        //                    continue;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Server has disconneted suddenly: {ex.Message}");
        //                serverConnection = false;
        //                break;

        //            }
        //        }
        //    }
        //}


        public bool TestConnection()
        {
            return client.Connected;
        }


        //public void SendDataToServer(string data)
        //{
        //    try
        //    {

        //        BinaryFormatter formatter = new BinaryFormatter();
        //        using (NetworkStream stream = client.GetStream())
        //        {
        //            formatter.Serialize(stream, data);
        //            Console.WriteLine("Data sent to server: " + data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error sending data to server: " + ex.Message);
        //    }
        //}

        //public Dictionary<string, object> GetDataFromServer()
        //{
        //    NetworkStream clientStream = client.GetStream();
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    return (Dictionary<string, object>)binaryFormatter.Deserialize(clientStream);
        //}

        //public void SendObjToServer(object data)
        //{
        //    NetworkStream clientStream = client.GetStream();
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    binaryFormatter.Serialize(clientStream, data);
        //}

        //public bool GetAckFromServer()
        //{
        //    NetworkStream clientStream = client.GetStream();
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    return (bool)binaryFormatter.Deserialize(clientStream);
        //}

    }


}
