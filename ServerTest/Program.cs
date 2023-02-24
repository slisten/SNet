using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SNet;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer<ServerSession> server = new TcpServer<ServerSession>();
            server.Start("127.0.0.1",17666);
            
            while(true) {
                string ipt = Console.ReadLine();
                if(ipt == "quit") {
                    server.CloseServer();
                    break;
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(ipt);
                    List<ServerSession> sessionLst = server.GetSessionLst();
                    for(int i = 0; i < sessionLst.Count; i++) {
                        sessionLst[i].SendMsg(data);
                    }
                }
            }
         
            // SyncTest();
            // AsyncTest();
        }

        private static void SyncTest()
        {
            int port = 2000;
            string host = "127.0.0.1";
 
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);
 
            Socket s　=　new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.ReceiveBufferSize = 1;
            s.Bind(ipe);
            s.Listen(0);
            Console.WriteLine("等待客户端连接");
 
            Socket temp = s.Accept();
            Console.WriteLine("建立连接");
            temp.ReceiveBufferSize = 1;
            // Thread.Sleep(10000);
            int receiveLength = 0;
            while (true)
            {
                Console.WriteLine("begin receive,size :"+temp.ReceiveBufferSize);
                byte[] recvBytes = new byte[1024*1024*1024];
                int bytes;
                Console.ReadLine();
                bytes = temp.Receive(recvBytes, recvBytes.Length, 0);
                temp.ReceiveBufferSize = 1;
                receiveLength += bytes;
                string recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);
                Console.WriteLine(recvStr);
                // Console.WriteLine("接收总长度："+receiveLength);
                Console.WriteLine("长度："+bytes);
            }
 
            s.Close();
            Console.ReadLine();
        }

        private static void AsyncTest()
        {
            int port = 2000;
            string host = "127.0.0.1";
 
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);
 
            Socket s　=　new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.ReceiveBufferSize = 2;
            s.Bind(ipe);
            s.Listen(0);
            Console.WriteLine("等待客户端连接");
 
            Socket temp = s.Accept();
            temp.ReceiveBufferSize = 2;
            Console.WriteLine("建立连接");
            // temp.ReceiveBufferSize = 1024*11;
            // Thread.Sleep(10000);
            int receiveLength = 0;
            while (true)
            {
                Console.WriteLine("begin receive,size :"+temp.ReceiveBufferSize);
                byte[] recvBytes = new byte[1024*1024*1024];
                Console.ReadLine();
                temp.BeginReceive(recvBytes, 0, recvBytes.Length, 0, delegate(IAsyncResult ar)
                {
                    int count = temp.EndReceive(ar);
                    receiveLength += count;
                    string recvStr = Encoding.UTF8.GetString(recvBytes, 0, count);
                    Console.WriteLine(recvStr);
                    Console.WriteLine("接收总长度："+receiveLength);
                },temp );
            }
 
            s.Close();
            Console.ReadLine();
        }

    }
}