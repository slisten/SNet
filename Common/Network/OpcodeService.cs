using System;
using System.Collections.Generic;

namespace Common
{
    public class OpcodeService: IOpcode
    {
        [Inject]
        private ILog log;

        [Inject]
        private IAttribute attributeService;
        
        private Dictionary<Type, NetOpcode> type2Opcode = new Dictionary<Type, NetOpcode>();
        private Dictionary<NetOpcode, Type> opcode2Type = new Dictionary<NetOpcode, Type>();

        public void OnInit()
        {
            type2Opcode.Clear();
            opcode2Type.Clear();
            List<Type> types = attributeService.GetTypes<OpcodeAttribute>();
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(OpcodeAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                OpcodeAttribute messageAttribute = attrs[0] as OpcodeAttribute;
                if (messageAttribute == null)
                {
                    continue;
                }

                type2Opcode.Add(type, messageAttribute.Opcode);
                opcode2Type.Add(messageAttribute.Opcode,type);
            }
        }
        
        public NetOpcode GetOpcode(Type messageType)
        {
            if (type2Opcode.TryGetValue(messageType, out NetOpcode val))
            {
                return val;
            }
            log.LogWarning("不存在Opcode:"+messageType);
            return NetOpcode.None;
        }
        
        public Type GetType(NetOpcode opcode)
        {
            if (opcode2Type.TryGetValue(opcode, out Type val))
            {
                return val;
            }
            log.LogWarning("不存在Opcode:"+opcode);
            return null;
        }

        public void OnDestroy()
        {
            
        }
    }
}