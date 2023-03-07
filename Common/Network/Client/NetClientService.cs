using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SNet;

namespace Common
{
    public enum ClientState
    {
        Disconnect,
        Connecting,
        Connected
    }
    public class NetClientService: INetClient
    {
        [Inject]
        private ILog log;

        [Inject]
        private IAttribute attributeService;

        [Inject]
        private IOpcode opcodeService;

        [Inject]
        private INetMsgDispatch netMsgDispatchService;

        public Dictionary<int, Action<MsgRpcResponse>> RequestCallback { get; set; }
        private TcpClient<ClientSession> tcpClient;
        public ClientState State;
        private int rpcID;
        public void OnInit()
        {
            rpcID = 0;
            State = ClientState.Disconnect;
            RequestCallback=new Dictionary<int, Action<MsgRpcResponse>>();
            tcpClient = new TcpClient<ClientSession>();
        }

        

        public void Connect(string ip,int port)
        {
            if (State == ClientState.Connected)
            {
                log.LogWarning("已经建立连接");
                return;
            }

            if (State == ClientState.Connecting)
            {
                log.LogWarning("正在建立连接");
                return;
            }
            State = ClientState.Connecting;
            tcpClient.Start(ip,port);
        }

        public Task<MsgRpcResponse> SendRpc(MsgRpcRequest msg)
        {
            int tmpRpcID = ++rpcID;
            msg.RpcID = tmpRpcID;
            var tcs = new TaskCompletionSource<MsgRpcResponse>();
            this.RequestCallback[tmpRpcID] = (response) =>
            {
                try
                {
                    tcs.SetResult(response);
                }
                catch (Exception e)
                {
                    tcs.SetException(new Exception($"Rpc Error: {msg.GetType().FullName}", e));
                }
            };
            Send(msg);
            return tcs.Task;
        }

        public void Send(MsgBase msg)
        {
            try
            {
                NetOpcode opcode = opcodeService.GetOpcode(msg.GetType());
                if (opcode == NetOpcode.None)
                {
                    log.LogError("没有注册Opcode");
                    return;
                }
                string json = JsonConvert.SerializeObject(msg);
                byte[] opcodeBytes = BitConverter.GetBytes((int)opcode);
                byte[] msgBytes = Encoding.UTF8.GetBytes(json);
                int length = opcodeBytes.Length + msgBytes.Length;
                byte[] lengthBytes = BitConverter.GetBytes(length);
                byte[] sendBytes = new byte[length + lengthBytes.Length];
                Array.Copy(lengthBytes,0,sendBytes,0,lengthBytes.Length);
                Array.Copy(opcodeBytes,0,sendBytes,4,opcodeBytes.Length);
                Array.Copy(msgBytes,0,sendBytes,8,msgBytes.Length);
                Console.WriteLine("发送长度："+sendBytes.Length);
                tcpClient.Session.SendMsg(sendBytes);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
        }

        public void OnConnected()
        {
            State = ClientState.Connected;
        }
        
        public void OnDisConnected()
        {
            State = ClientState.Disconnect;
        }
        
        public void OnReceive(Session session, byte[] data)
        {
            NetOpcode opcode;
            string msg = PackageHandler.Write(data,out opcode);

            if (msg == null || opcode == NetOpcode.None)
            {
                return;
            }
            
            //分发
            netMsgDispatchService.Handle(tcpClient.Session,opcode,msg);
            //rpc回调
            // RpcMsgBase rpcMsg = JsonConvert.DeserializeObject<RpcMsgBase>(msg);
            //
            // if (rpcMsg == null)
            // {
            //     return;
            // }
            // Action<RpcMsgBase> action;
            // if (!this.requestCallback.TryGetValue(rpcMsg.RpcID, out action))
            // {
            //     return;
            // }
            // this.requestCallback.Remove(rpcMsg.RpcID);
            //
            // action(rpcMsg);
        }

        public void OnDestroy()
        {
            tcpClient.Close();
        }
    }
}