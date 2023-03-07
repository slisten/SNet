using System;
using Newtonsoft.Json;
using SNet;

namespace Common
{
    public abstract class NetMessageHandlerBase<T> : INetMessageHandler where T: class
    {
        public abstract void Run(Session session, T message);

        public void Handle(Session session, string msg)
        {
            T message = JsonConvert.DeserializeObject<T>(msg);
            if (message == null)
            {
                Env.CurEnv.GetService<ILog>().LogError("消息类型转换错误");
                // Log.Error($"消息类型转换错误: {msg.GetType().Name} to {typeof(Message).Name}");
            }
            this.Run(session, message);


            //rpc回调
            
            MsgRpcResponse rpcMsg=message as MsgRpcResponse;
            if (rpcMsg != null)
            {
                Action<MsgRpcResponse> action;
                if (!Env.CurEnv.GetService<INetClient>().RequestCallback.TryGetValue(rpcMsg.RpcID, out action))
                {
                    return;
                }
                Env.CurEnv.GetService<INetClient>().RequestCallback.Remove(rpcMsg.RpcID);

                action(rpcMsg);
            }

        }
        public Type GetMessageType()
        {
            return typeof(T);
        }
    }
}