using System;
using SNet;

namespace Common
{
    public class ClientSession: Session
    {
        public Action ConnectedCB;
        public Action DisConnectedCB;
        public Action<Session,byte[]> ReceiveCB;

        private INetClient netClient;
        public ClientSession()
        {
            netClient = Env.CurEnv.GetService<INetClient>();
            ConnectedCB += netClient.OnConnected;
            DisConnectedCB += netClient.OnDisConnected;
            ReceiveCB += netClient.OnReceive;
        }
        
        public override void OnConnected()
        {
            base.OnConnected();
            Env.CurEnv.GetService<ILog>().Log("连接成功");
            ConnectedCB?.Invoke();
        }

        public override void OnDisConnected()
        {
            base.OnDisConnected();
            Env.CurEnv.GetService<ILog>().Log("断开连接");
            DisConnectedCB?.Invoke();
        }

        public override void OnReceive(Session session, byte[] data)
        {
            base.OnReceive(session, data);
            // Env.CurEnv.GetService<ILog>().Log("接收数据");
            ReceiveCB?.Invoke(session,data);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ConnectedCB -= netClient.OnConnected;
            DisConnectedCB -= netClient.OnDisConnected;
            ReceiveCB -= netClient.OnReceive;
        }
    }
}