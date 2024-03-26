//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using KitapClientMananger;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Diagnostics.Eventing.Reader;

//namespace Appclientconsole
//{

//    public class Connection
//    {
//        private ClientManager clientManager;
//        private bool connected = false;

//        public bool Connected
//        {
//            get { return connected; }
//            set { connected = value; }
//        }

//        public Connection()
//        {
//            clientManager = ClientManager.Instance;
//        }

//        public void Connectioninit()
//        {

//            //clientManager.connectRequest += SendConnectionRequest;
//            //clientManager.heartbeatThread += SendHeartbeat;

//            if (!clientManager.Connected)
//            {
//                ConnectToServer();
//            }
//        }

//        private void SendHeartbeat(CallBack statusObj)
//        {
//            Task.Run(() => clientManager.SendHeartbeat(GetStatus));
//        }

//        private void SendConnectionRequest()
//        {
//            clientManager.ConnectToServer();
//        }
//        private void ConnectToServer()
//        {
//            try
//            {
//                //connectRequest();
//                //if (clientManager.Connected)
//                //{
//                //    Task.Run(() => heartbeatThread(GetStatus));
//                //}
//            }

//            catch (SocketException e)
//            {
//                Console.WriteLine("Server not available");
//            }

//        }

//        public bool testServerConnection()
//        {
//            return clientManager.TestConnection();
//        }

//        public async void HeartbeatConnectionAsync()
//        {

//            Task.Run(() => clientManager.SendHeartbeat(GetStatus));


//        }

//        private void GetStatus(bool status)
//        {
//            Connected = status;
//        }
//        ///////////////////////////////////////////////////////////////////
//        ////////////////////////////////////////////////////////
//        //public delegate void ConnectRequest();
//        //public delegate void DisconnectRequest();
//        //public delegate void HeartbeatThread(CallBack statusObj);
//        //public class Connection
//        //{
//        //    private ClientManager clientManager;
//        //    private bool connected = false;
//        //    public ConnectRequest connectRequest;
//        //    public DisconnectRequest disconnectRequest;
//        //    public HeartbeatThread heartbeatThread;


//        //    public bool Connected
//        //    {
//        //        get { return connected; }
//        //        set { connected = value; }
//        //    }

//        //    public Connection()
//        //    {
//        //        clientManager = ClientManager.Instance;
//        //    }

//        //    public void Connectioninit()
//        //    {

//        //        connectRequest += clientManager.ConnectToServer;
//        //        heartbeatThread += clientManager.SendHeartbeat;

//        //        if (!clientManager.Connected)
//        //        {
//        //            ConnectToServer();
//        //        }
//        //    }

//        //    private void ConnectToServer()
//        //    {
//        //        try
//        //        {
//        //            connectRequest();
//        //            if (clientManager.Connected)
//        //            {
//        //                Task.Run(() => heartbeatThread(GetStatus));
//        //            }
//        //        }

//        //        catch (SocketException e)
//        //        {
//        //            Console.WriteLine("Server not available");
//        //        }

//        //    }

//        //    public bool testServerConnection()
//        //    {
//        //        return clientManager.TestConnection();
//        //    }

//        //    public async void HeartbeatConnectionAsync()
//        //    {

//        //        Task.Run(() => clientManager.SendHeartbeat(GetStatus));


//        //    }

//        //    private void GetStatus(bool status)
//        //    {
//        //        Connected = status;
//        //    }
//        ///////////////////////////////////////////////////////////////////////////////
//        /////////////////////////////////////////////
//        //private ClientManager clientManager;
//        //private bool connected = false;

//        //public bool Connected { get { return connected;} set { connected = value;} }
//        //public Connection() {clientManager = ClientManager.Instance;}

//        //public void Connectioninit()
//        //{
//        //    try
//        //    {
//        //        clientManager.connectRequest += clientManager.ConnectToServer;
//        //        clientManager.heartbeatThread += clientManager.SendHeartbeat;

//        //        if (!clientManager.TestConnection())
//        //        {
//        //            SetupServerConnection();
//        //        }
//        //    } 

//        //    catch 

//        //    { Console.WriteLine("Server not available"); }
//        //}

//        //private void SetupServerConnection()
//        //{
//        //    clientManager.connectRequest();
//        //    Task.Run(() => clientManager.heartbeatThread(GetStatus));
//        //}


//        //private void GetStatus(bool status)
//        //{
//        //    Connected = status;
//        //}

//        /////////////////////////////////////////////////////////////
//        ///
//        //private ClientManager clientManager;
//        //private bool connected = false;

//        //public bool Connected
//        //{
//        //    get { return connected; }
//        //    set { connected = value; }
//        //}

//        //public Connection()
//        //{
//        //    clientManager = ClientManager.Instance;
//        //}

//        //public void Connectioninit()
//        //{

//        //}


//        //public void ConnectToServer()
//        //{
//        //    clientManager.ConnectToServer();
//        //}

//        //public bool testServerConnection()
//        //{
//        //    return clientManager.TestConnection();
//        //}

//        //public async void HeartbeatConnectionAsync()
//        //{

//        //    Task.Run(() => clientManager.SendHeartbeat(GetStatus));


//        //}

//        //private void GetStatus(bool status)
//        //{
//        //    Connected = status;
//        //}



//        ///////////////////////////////////////////////////////////////

//        //public async Task SetUpHeartbeatAsync()
//        //{
//        //    await Task.Run(() => clientManager.SendHeartbeat());
//        //    Console.WriteLine("Server is avaiable");
//        //}

//        //public Connection()
//        //{
//        //    try
//        //    {
//        //        clientManager = ClientManager.Instance;
//        //        clientManager.ConnectToServer();
//        //        Task.Run(() => clientManager.SendHeartbeat());
//        //    }
//        //    catch
//        //    {
//        //        Console.WriteLine("Server not available");
//        //    }

//        //}

//        //public async void checkConnectivity()
//        //{
//        //    while (true)
//        //    {
//        //        if (!clientManager.ServerConnection)
//        //        {
//        //            Connected = false;
//        //        }
//        //        else
//        //        {
//        //            Connected = true;
//        //        }
//        //    }
//        //}

//        //while (true)
//        //    {
//        //        if (!clientManager.ServerConnection)
//        //        {
//        //            Connected = false;
//        //        }
//        //        else
//        //        {
//        //            Connected = true;
//        //        }
//        //    }

//    }
//}
