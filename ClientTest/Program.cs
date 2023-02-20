using System;
using System.Text;
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
                    client.session.SendMsg(msg);
                }
            }
        }
    }
}