using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SNet;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient<ClientSession> client = new TcpClient<ClientSession>();
            client.Start("127.0.0.1", 17666);
            while(true) {
                string ipt = Console.ReadLine();
                if(ipt == "quit") {
                    client.Close();
                    break;
                }
                else
                {
                    byte[] msg = Encoding.UTF8.GetBytes(ipt);
                    LogHelper.Log("长度；"+msg.Length);
                    client.session.SendMsg(msg);
                }
            }
         
            // SyncTest();
            // AsyncTest();
        }

        private static void SyncTest()
        {
            try
            {
                int port = 2000;
                string host = "127.0.0.1";
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
 
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Conneting…");
                c.SendBufferSize = 1;
                c.Connect(ipe);

                Thread th = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        string recvStr = "";
                        byte[] recvBytes = new byte[1024];
                        int bytes;
                        bytes = c.Receive(recvBytes, recvBytes.Length, 0); 
                        if (bytes > 0)
                        {
                            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
                            Console.WriteLine("client get message:{0}", recvStr);
                        }
                        
                    }
                }));
                th.IsBackground = true;
                th.Start();
                while (true)
                {
                    string ipt = Console.ReadLine();
                    if(ipt == "quit") {
                        c.Close();
                        break;
                    }
                    else if (ipt=="test")
                    {
                        int totalLength = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            string cn = "[1]0123456789" +
                                        "[2]0123456789" +
                                        "[3]0123456789" +
                                        "[4]0123456789" +
                                        "[5]0123456789" +
                                        "[6]0123456789" +
                                        "[7]0123456789" +
                                        "[8]0123456789" +
                                        "[9]0123456789" +
                                        "[10]0123456789";
                            cn = string.Format("=={0}==\\n" + cn,i);
                            byte[] msg = Encoding.UTF8.GetBytes(cn);
                            totalLength += msg.Length;
                            
                            int count = c.Send(msg,msg.Length,0);
                            LogHelper.Log("长度；"+totalLength+" size "+c.SendBufferSize+" index: "+i+"cur Count :"+count);
                        }
                    }
                    else
                    {
                        byte[] msg = Encoding.UTF8.GetBytes(ipt);
                        int count = c.Send(msg,msg.Length,0);
                        LogHelper.Log("长度；"+msg.Length+" 实际长度 "+count);
                    }
                }
                c.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("argumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }

        private static void AsyncTest()
        {
            try
            {
                int port = 2000;
                string host = "127.0.0.1";
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
 
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Conneting…");
                c.SendBufferSize = 2;
                c.Connect(ipe);

                Thread th = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        string recvStr = "";
                        byte[] recvBytes = new byte[1024];
                        int bytes;
                        bytes = c.Receive(recvBytes, recvBytes.Length, 0); 
                        if (bytes > 0)
                        {
                            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
                            Console.WriteLine("client get message:{0}", recvStr);
                        }
                        
                    }
                }));
                th.IsBackground = true;
                th.Start();
                while (true)
                {
                    string ipt = Console.ReadLine();
                    if(ipt == "quit") {
                        c.Close();
                        break;
                    }
                    else if (ipt=="test")
                    {
                        int totalLength = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            string cn = "[1]0123456789" +
                                        "[2]0123456789" +
                                        "[3]0123456789" +
                                        "[4]0123456789" +
                                        "[5]0123456789" +
                                        "[6]0123456789" +
                                        "[7]0123456789" +
                                        "[8]0123456789" +
                                        "[9]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789" +
                                        "[10]0123456789"+
                                        "0123456789";
                            cn = string.Format("=={0}==\\n" + cn,i);
                            byte[] msg = Encoding.UTF8.GetBytes(cn);
                            
                            LogHelper.Log("send :"+i);
                            // LogHelper.Log("长度；"+totalLength+" size "+c.SendBufferSize);
                            c.BeginSend(msg, 0, msg.Length, 0, delegate(IAsyncResult ar)
                            {
                                int count = c.EndSend(ar);
                                if (count < msg.Length)
                                {
                                    Console.WriteLine("数据未发送完整==========================================");
                                }

                                if (count == 0)
                                {
                                    Console.WriteLine("数据长度为0=============================================");
                                }
                                else
                                {
                                    totalLength += count;
                                    LogHelper.Log("长度；"+totalLength+" size "+c.SendBufferSize+"index :"+i);
                                    Console.WriteLine("发送成功");
                                }
                                
                            }, c);
                        }
                    }
                    else
                    {
                        byte[] msg = Encoding.UTF8.GetBytes(ipt);
                        LogHelper.Log("长度；"+msg.Length+" size "+c.SendBufferSize);
                        c.Send(msg,msg.Length,0);
                    }
                }
                c.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("argumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
    }
}