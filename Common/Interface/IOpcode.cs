using System;

namespace Common
{
    public interface IOpcode: IService
    {
        NetOpcode GetOpcode(Type messageType);
        Type GetType(NetOpcode opcode);
    }
}