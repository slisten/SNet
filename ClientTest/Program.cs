using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using SNet;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Env env = Env.NewEnv();
            env.BindService<ILog>(new DefaultLogService());
            env.BindService<IAttribute>(new AttributeService());
            env.GetService<IAttribute>().Add(typeof(Env).Assembly);
            env.GetService<IAttribute>().Add(typeof(Program).Assembly);
            
            env.BindService<IOpcode>(new OpcodeService());
            env.BindService<INetMsgDispatch>(new NetMsgDispatchService());
            env.BindService<INetClient>(new NetClientService());
            env.GetService<INetClient>().Connect("127.0.0.1",10029);
            

            while(true) {
                string ipt = Console.ReadLine();
                if(ipt == "quit") {
                    env.OnDestroy();
                    break;
                }
                else if(ipt=="test")
                {
                    Test();
                }
                else
                {
                    string ms = "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没" +
                                "你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没你吃了没";
                    int count = Convert.ToInt32(ipt);
                    for (int i = 0; i < count; i++)
                    {
                        ms += ms;
                    }

                    ms += "100";
                    Chat(ms);
                }
            }
        }


        private static async void Test()
        {
            TestRequest msg = new TestRequest();
            msg.Name = "listen";
            msg.Age = 18;
            TestResponse response = await Env.CurEnv.GetService<INetClient>().SendRpc(msg) as TestResponse;
            Console.WriteLine("Await收到回复："+response.Desc);
        }
        
        private static async void Chat(string content)
        {
            TestChatRequest msg = new TestChatRequest();
            msg.Content = content;
            TestChatResponse response = await Env.CurEnv.GetService<INetClient>().SendRpc(msg) as TestChatResponse;
            if (response.Result == ErrorCode.Success)
            {
                Console.WriteLine("Await返回成功");
            }
        }
    }
}