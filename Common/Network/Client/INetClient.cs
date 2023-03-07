using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SNet;

namespace Common
{
    public interface INetClient: IService
    {
        Dictionary<int, Action<MsgRpcResponse>> RequestCallback { set; get; }
        void Connect(string ip, int port);
        Task<MsgRpcResponse> SendRpc(MsgRpcRequest msg);
        void Send(MsgBase msg);
        void OnConnected();
        void OnDisConnected();
        void OnReceive(Session session, byte[] data);
    }
}