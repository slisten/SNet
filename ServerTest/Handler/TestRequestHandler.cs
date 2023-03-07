using System;
using Common;
using Common.Server;
using SNet;

namespace ServerTest.Handler
{
    [MessageHandler]
    public class TestRequestHandler: NetMessageHandlerBase<TestRequest>
    {
        // public override void Handle(Session session, TestRequest message)
        // {
        //     TestRequest request=message as TestRequest;
        //     Console.WriteLine("收到消息："+request.Name+request.Age);
        //     TestResponse response = new TestResponse();
        //     response.Desc = "已收到";
        //     ServerSession serverSession=session as ServerSession;
        //     serverSession.Send(response);
        // }

        public override void Run(Session session, TestRequest request)
        {
            // TestRequest request=message as TestRequest;
            Console.WriteLine("收到消息："+request.Name+request.Age);
            TestResponse response = new TestResponse();
            response.RpcID = request.RpcID;
            response.Desc = "已收到";
            ServerSession serverSession=session as ServerSession;
            serverSession.Send(response);
        }
    }
}