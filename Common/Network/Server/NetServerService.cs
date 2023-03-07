using SNet;

namespace Common.Server
{
    public class NetServerService: INetServer
    {
        private TcpServer<ServerSession> tcpServer;

        public void OnInit()
        {
            
        }
        public void Listen(string ip,int port)
        {
            tcpServer = new TcpServer<ServerSession>();
            tcpServer.Start(ip,port);
        }

        public void OnDestroy()
        {
            tcpServer.Close();
        }
    }
}