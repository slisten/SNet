using System;
using System.Net.Sockets;

namespace SNet
{
    public enum AsyncSessionState {
        None,
        Connected,
        DisConnected
    }
    public class Session
    {
        private Socket skt;
        private Action closeCB;
        public AsyncSessionState sessionState = AsyncSessionState.None;
        public bool IsConnected {
            get {
                return sessionState == AsyncSessionState.Connected;
            }
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
        
        public void Start(Socket skt,Action closeCB)
        {
            try {
                this.skt = skt;
                this.closeCB = closeCB;
                skt.BeginReceive(buffer.Buffer, 0, buffer.Capacity, SocketFlags.None, new AsyncCallback(ReceiveData),null);
                sessionState = AsyncSessionState.Connected;
                OnConnected();
            }
            catch(Exception e) {
                LogHelper.Error(e.Message);
                OnDisConnected();
            }
        }
        
        private void ReceiveData(IAsyncResult ar)
        {
            try {
                if(skt == null || skt.Connected == false) {
                    LogHelper.Warn("Socket is null or not connected.");
                    return;
                }
                
                int len = skt.EndReceive(ar);
                
                if(len == 0) {
                    LogHelper.ColorLog(AsyncLogColor.Yellow, "远程连接正常下线");
                    Close();
                    return;
                }

                byte[] tmpBuffer = new byte[len];
                Array.Copy(buffer.Buffer,tmpBuffer,len);
                OnReceive(tmpBuffer);
                //判断接收长度是否等于Capacity
                buffer.CheckBuffer(len);
                skt.BeginReceive(buffer.Buffer, 0, buffer.Capacity, SocketFlags.None, new AsyncCallback(ReceiveData),null);
            }
            catch(Exception e) {
                LogHelper.Warn("RcvHeadWarn:{0}", e.ToString());
                Close();
            }
        }
        
        public bool SendMsg(byte[] data) {
            bool result = false;
            if(sessionState != AsyncSessionState.Connected) {
                LogHelper.Warn("Connection is Disconnected.can not send net msg.");
            }
            else {
                NetworkStream ns = null;
                try {
                    ns = new NetworkStream(skt);
                    if(ns.CanWrite) {
                        ns.BeginWrite(
                            data,
                            0,
                            data.Length,
                            new AsyncCallback(SendCB),
                            ns);
                    }
                    result = true;
                }
                catch(Exception e) {
                    LogHelper.Error("SndMsgNSError:{0}.", e.Message);
                }
            }
            return result;
        }
        private void SendCB(IAsyncResult ar) {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            try {
                ns.EndWrite(ar);
                ns.Flush();
                ns.Close();
            }
            catch(Exception e) {
                LogHelper.Error("SndMsgNSError:{0}.", e.Message);
            }
        }
        
        public void Close() {
            sessionState = AsyncSessionState.DisConnected;
            OnDisConnected();

            closeCB?.Invoke();
            try {
                if(skt != null) {
                    skt.Shutdown(SocketShutdown.Both);
                    skt.Close();
                    skt = null;
                }
            }
            catch(Exception e) {
                LogHelper.Error("ShutDown Socket Error:{0}", e.Message);
            }
        }
    }
}