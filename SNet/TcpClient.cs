using System;
using System.Net;
using System.Net.Sockets;

namespace SNet
{
    public class TcpClient<T> where T: Session,new()
    {
        public T session;
        private Socket socket = null;
        
        public void Start(string ip, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                LogHelper.ColorLog(AsyncLogColor.Green, "Client Start...");
                EndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.BeginConnect(pt, new AsyncCallback(OnConnect), null);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            session = new T();
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    session.Start(socket, null);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
        }

        public void Close()
        {
            if (session != null)
            {
                session.Close();
                session = null;
            }

            if (socket != null)
            {
                socket = null;
            }
        }
    }
}