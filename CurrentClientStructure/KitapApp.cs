//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Appclientconsole
//{
//    public class KitapApp
//    {
//        private Connection connection;
//        public KitapApp()
//        {
//            connection = new Connection();       
//        }


//        public async void initAsync()
//        {
//            if (!connection.Connected)
//            {
//                connection.Connectioninit();

//                while (connection.Connected)
//                {
//                    Console.WriteLine("bxcbcvbvcbcvbcvb");
//                    //server is connected 
//                }
//            }
//        }


//        //public async void initAsync()
//        //{
//        //    while (true)
//        //    {

//        //        try
//        //        {
//        //            connection.ConnectToServer();

//        //            if (connection.testServerConnection())
//        //            {
//        //                Task.Run(() => connection.HeartbeatConnectionAsync());

//        //                while (connection.Connected)
//        //                {
//        //                    Console.WriteLine("bxcbcvbvcbcvbcvb");
//        //                    //server is connected 
//        //                }
//        //                Console.WriteLine("Disconnected");
//        //                //server is connected 
//        //                //server has disconnected

//        //            }

//        //        }
//        //        catch (SocketException ex)
//        //        {
//        //            Console.WriteLine("Server is not avaible");
//        //            Thread.Sleep(2000);
//        //        }

//        //    }

//        //}
//        //public KitapApp()
//        //{
//        //    connection = new Connection();
//        //    Task.Run(() => connection.checkConnectivity());
//        //}
//        //public async void initAsync()
//        //{
//        //    try
//        //    {
//        //        while (true)
//        //        {
//        //            if (!connection.Connected)
//        //            {
//        //                Console.WriteLine("Server has disconnected");
//        //            }

//        //            else { }
//        //        }

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Console.WriteLine("Server not avaible");
//        //    }
//        //}

//    }
//}
