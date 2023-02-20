using System.Text;
using SNet;

namespace ServerTest
{
    public class ServerSession: Session
    {
        public override void OnConnected()
        {
            base.OnConnected();
            LogHelper.Log("新增客户端连接："+this.GetHashCode());
        }

        public override void OnDisConnected()
        {
            base.OnDisConnected();
            LogHelper.Log("客户端断开连接："+this.GetHashCode());
        }

        public override void OnReceive(byte[] data)
        {
            base.OnReceive(data);
            LogHelper.Log("接收数据："+ this.GetHashCode() + " |" +Encoding.UTF8.GetString(data));
        }
    }
}