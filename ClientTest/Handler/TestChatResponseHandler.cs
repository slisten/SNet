using Common;
using SNet;

namespace ClientTest.Handler
{
    [MessageHandler]
    public class TestChatResponseHandler: NetMessageHandlerBase<TestChatResponse>
    {
        public override void Run(Session session, TestChatResponse message)
        {
            if (message.Result == ErrorCode.Success)
            {
                Env.CurEnv.GetService<ILog>().Log("Handler返回成功");
            }
        }
    }
}