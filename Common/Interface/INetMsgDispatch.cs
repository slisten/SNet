using SNet;

namespace Common
{
    public interface INetMsgDispatch: IService
    {
        void RegisterHandler(NetOpcode opcode, INetMessageHandler handler);
        void Handle(Session session, NetOpcode opcode, string msg);
    }
}