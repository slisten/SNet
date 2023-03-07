using System;
using System.Collections.Generic;
using SNet;

namespace Common
{
    public class NetMsgDispatchService: INetMsgDispatch
    {
        [Inject]
        private ILog log;
        
        [Inject]
        private IAttribute attributeService;

        [Inject]
        private IOpcode opcodeService;

        private readonly Dictionary<NetOpcode, List<INetMessageHandler>> handlers = new Dictionary<NetOpcode, List<INetMessageHandler>>();

        public void OnInit()
        {
            handlers.Clear();

            List<Type> types = attributeService.GetTypes(typeof(MessageHandlerAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(MessageHandlerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                INetMessageHandler iMHandler = Activator.CreateInstance(type) as INetMessageHandler;
                if (iMHandler == null)
                {
                    log.LogError($"message handle {type.Name} 需要继承 INetMessageHandler");
                    continue;
                }

                Type messageType = iMHandler.GetMessageType();
                NetOpcode opcode = opcodeService.GetOpcode(messageType);
                if (opcode == 0)
                {
                    log.LogError($"消息opcode为0: {messageType.Name}");
                    continue;
                }
                RegisterHandler(opcode, iMHandler);
            }
        }

        public void RegisterHandler(NetOpcode opcode, INetMessageHandler handler)
        {
            if (!handlers.ContainsKey(opcode))
            {
                handlers.Add(opcode, new List<INetMessageHandler>());
            }
            handlers[opcode].Add(handler);
        }

        public void Handle(Session session, NetOpcode opcode,string msg)
        {
            List<INetMessageHandler> actions;
            if (!handlers.TryGetValue(opcode, out actions))
            {
                log.LogError($"消息没有处理: {opcode}");
                return;
            }
			
            foreach (INetMessageHandler ev in actions)
            {
                try
                {
                    ev.Handle(session, msg);
                }
                catch (Exception e)
                {
                    log.LogError(e.ToString());
                }
            }
        }

        public void OnDestroy()
        {
            
        }
    }
}