namespace Common
{
    public class OpcodeAttribute: AttributeBase
    {
        public NetOpcode Opcode { get; }

        public OpcodeAttribute(NetOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}