using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SNet
{
    public class TcpServer<T> where T: Session,new()
    {
        private int backlog = 10;
        private List<T> sessionList = null;
        private Socket listenSocket = null;
        public void Start(string ip, int port)
        {
            sessionList = new List<T>();
            try
            {
                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenSocket.NoDelay = true; 
                listenSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                listenSocket.Listen(backlog);
                LogHelper.ColorLog(AsyncLogColor.Green, "Server Start...");
                listenSocket.BeginAccept(OnClientConnect, null);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            T session = new T();
            try
            {
                Socket clientSocket = listenSocket.EndAccept(ar);
                if (clientSocket.Connected)
                {
                    lock (sessionList)
                    {
                        sessionList.Add(session);
                    }

                    session.Start(clientSocket, () =>
                    {
                        if (sessionList.Contains(session))
                        {
                            lock (sessionList)
                            {
                                if (sessionList.Remove(session))
                                {
                                    LogHelper.ColorLog(AsyncLogColor.Yellow, "Clear ServerSession Success.");
                                }
                                else
                                {
                                    LogHelper.ColorLog(AsyncLogColor.Yellow, "Clear ServerSession Fail.");
                                }
                            }
                        }
                    });
                }

                listenSocket.BeginAccept(OnClientConnect, null);
            }
            catch (Exception e)
            {
                LogHelper.Error("ClientConnectCB:{0}", e.Message);
            }
        }

        public List<T> GetSessionLst()
        {
            return sessionList;
        }

        public void Close()
        {
            for (int i = 0; i < sessionList.Count; i++)
            {
                sessionList[i].Close();
            }

            sessionList = null;
            if (listenSocket != null)
            {
                listenSocket.Close();
                listenSocket = null;
            }
        }
    }
}