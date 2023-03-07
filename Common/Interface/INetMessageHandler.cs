using System;
using SNet;

namespace Common
{
    public interface INetMessageHandler
    {
        void Handle(Session session, string message);
        Type GetMessageType();
    }
}