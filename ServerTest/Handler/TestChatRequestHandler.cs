using Common;
using Common.Server;
using SNet;

namespace ServerTest.Handler
{
    [MessageHandler]
    public class TestChatRequestHandler: NetMessageHandlerBase<TestChatRequest>
    {
        public override void Run(Session session, TestChatRequest message)
        {
            Env.CurEnv.GetService<ILog>().Log(session.GetRemoteEndPoint()+": "+message.Content);
            TestChatResponse response = new TestChatResponse();
            response.RpcID = message.RpcID;
            response.Result = ErrorCode.Success;
            ServerSession serverSession=session as ServerSession;
            serverSession.Send(response);
        }
    }
}