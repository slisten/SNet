using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SNet
{
    public class TcpServer<T> where T: Session,new()
    {
        public int backlog = 10;
        List<T> sessionLst = null;
        private Socket skt = null;
        public void Start(string ip, int port)
        {
            sessionLst = new List<T>();
            try
            {
                skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                skt.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                skt.Listen(backlog);
                LogHelper.ColorLog(AsyncLogColor.Green, "Server Start...");
                skt.BeginAccept(OnClientConnect, null);
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
                Socket clientSkt = skt.EndAccept(ar);
                if (clientSkt.Connected)
                {
                    lock (sessionLst)
                    {
                        sessionLst.Add(session);
                    }

                    session.Start(clientSkt, () =>
                    {
                        if (sessionLst.Contains(session))
                        {
                            lock (sessionLst)
                            {
                                if (sessionLst.Remove(session))
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

                skt.BeginAccept(OnClientConnect, null);
            }
            catch (Exception e)
            {
                LogHelper.Error("ClientConnectCB:{0}", e.Message);
            }
        }

        public List<T> GetSessionLst()
        {
            return sessionLst;
        }

        public void CloseServer()
        {
            for (int i = 0; i < sessionLst.Count; i++)
            {
                sessionLst[i].Close();
            }

            sessionLst = null;
            if (skt != null)
            {
                skt.Close();
                skt = null;
            }
        }
    }
}