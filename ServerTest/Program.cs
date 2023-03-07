using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Server;
using SNet;

namespace ServerTest
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
            env.BindService<INetServer>(new NetServerService());
            
            env.GetService<INetServer>().Listen("127.0.0.1",10029);
            

            while(true) {
                string ipt = Console.ReadLine();
                if(ipt == "quit") {
                    Env.CurEnv.OnDestroy();
                    break;
                }
                else
                {
                    
                }
            }
         
        }
    }
}