using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SNet
{
    public enum AsyncSessionState
    {
        None,
        Connected,
        DisConnected
    }

    public class Session
    {
        private Socket socket;
        private Action OnCloseCallback;
        public AsyncSessionState sessionState = AsyncSessionState.None;

        public bool IsConnected
        {
            get { return sessionState == AsyncSessionState.Connected; }
        }


        private DataBuffer buffer = new DataBuffer();

        public virtual void OnReceive(byte[] data)
        {
        }

        public virtual void OnConnected()
        {
        }

        public virtual void OnDisConnected()
        {
        }

        public void Start(Socket skt, Action closeCB)
        {
            try
            {
                this.socket = skt;
                this.OnCloseCallback = closeCB;
                skt.BeginReceive(buffer.Buffer, 0, buffer.Capacity, SocketFlags.None, ReceiveData, null);
                sessionState = AsyncSessionState.Connected;
                OnConnected();
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
                OnDisConnected();
            }
        }

        private void ReceiveData(IAsyncResult ar)
        {
            try
            {
                if (socket == null || socket.Connected == false)
                {
                    LogHelper.Warn("Socket is null or not connected.");
                    return;
                }

                int len = socket.EndReceive(ar);

                if (len == 0)
                {
                    LogHelper.ColorLog(AsyncLogColor.Yellow, "远程连接正常下线");
                    Close();
                    return;
                }

                byte[] tmpBuffer = new byte[len];
                Array.Copy(buffer.Buffer, tmpBuffer, len);
                OnReceive(tmpBuffer);
                //判断接收长度是否等于Capacity
                buffer.CheckBuffer(len);
                socket.BeginReceive(buffer.Buffer, 0, buffer.Capacity, SocketFlags.None, ReceiveData, null);
            }
            catch (Exception e)
            {
                LogHelper.Warn("RcvHeadWarn:{0}", e.ToString());
                Close();
            }
        }

        public void SendMsg(byte[] data)
        {
            if (sessionState != AsyncSessionState.Connected)
            {
                LogHelper.Warn("Connection is Disconnected.can not send net msg.");
            }
            else
            {
                NetworkStream ns = null;
                try
                {
                    socket.BeginSend(data, 0, data.Length, 0, OnSendCallback, null);

                }
                catch (Exception e)
                {
                    LogHelper.Error("SndMsgNSError:{0}.", e.Message);
                }
            }
        }

        private void OnSendCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch (Exception e)
            {
                LogHelper.Error("SndMsgNSError:{0}.", e.Message);
            }
        }

        public void Close()
        {
            sessionState = AsyncSessionState.DisConnected;
            OnDisConnected();

            OnCloseCallback?.Invoke();
            try
            {
                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error("ShutDown Socket Error:{0}", e.Message);
            }
        }

        public EndPoint GetLocalEndPoint()
        {
            return socket.LocalEndPoint;
        }

        public EndPoint GetRemoteEndPoint()
        {
            return socket.RemoteEndPoint;
        }
    }
}