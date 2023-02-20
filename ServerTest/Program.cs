using System;
using System.Collections.Generic;
using System.Text;
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
        }
    }
}