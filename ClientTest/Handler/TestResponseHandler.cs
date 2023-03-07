using System;
using Common;
using SNet;

namespace ClientTest.Handler
{
    [MessageHandler]
    public class TestResponseHandler: NetMessageHandlerBase<TestResponse>
    {
        // public override void Handle(Session session, TestResponse message)
        // {
        //     TestResponse response=message as TestResponse;
        //     Console.WriteLine("收到回复："+response.Desc);
        // }

        public override void Run(Session session, TestResponse response)
        {
            // TestResponse response=message as TestResponse;
            Console.WriteLine("Handler收到回复："+response.Desc);
        }
    }
}