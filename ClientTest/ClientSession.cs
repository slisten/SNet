using System.Text;
using SNet;

namespace ClientTest
{
    public class ClientSession: Session
    {
        public override void OnConnected()
        {
            base.OnConnected();
            LogHelper.Log("连接成功："+this.GetHashCode());
        }

        public override void OnDisConnected()
        {
            base.OnDisConnected();
            LogHelper.Log("断开连接："+this.GetHashCode());
        }

        public override void OnReceive(byte[] data)
        {
            base.OnReceive(data);
            LogHelper.Log("接收数据："+Encoding.UTF8.GetString(data));
        }
    }
}