using System;
using System.Net;
using System.Net.Sockets;

namespace SNet
{
    public class TcpClient<T> where T: Session,new()
    {
        public T Session;
        private Socket socket = null;
        
        public void Start(string ip, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //关闭nagle算法
                socket.NoDelay = true;  
                LogHelper.ColorLog(AsyncLogColor.Green, "Client Start...");
                EndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.BeginConnect(pt, OnConnect, null);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            Session = new T();
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    Session.Start(socket, null);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
        }

        public void Close()
        {
            if (Session != null)
            {
                Session.Close();
                Session = null;
            }

            if (socket != null)
            {
                socket = null;
            }
        }
    }
}