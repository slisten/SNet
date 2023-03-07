using System;
using System.Text;
using Newtonsoft.Json;
using SNet;

namespace Common.Server
{
    public class ServerSession: Session
    {
        public override void OnConnected()
        {
            base.OnConnected();
        }

        public override void OnDisConnected()
        {
            base.OnDisConnected();
        }

        public override void OnReceive(Session session, byte[] data)
        {
            base.OnReceive(session, data);
            
            Console.WriteLine("接收长度："+data.Length);
            NetOpcode opcode;
            string msg = PackageHandler.Write(data,out opcode);

            if (msg == null || opcode == NetOpcode.None)
            {
                return;
            }
            
            //分发
            Env.CurEnv.GetService<INetMsgDispatch>().Handle(this,opcode,msg);
        }
        
        public void Send(MsgBase msg)
        {
            try
            {
                NetOpcode opcode = Env.CurEnv.GetService<IOpcode>().GetOpcode(msg.GetType());
                if (opcode == NetOpcode.None)
                {
                    // log.LogError("没有注册Opcode");
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
                SendMsg(sendBytes);
            }
            catch (Exception e)
            {
                // log.LogError(e.ToString());
            }
        }
    }
}