namespace Common
{
    public enum ErrorCode
    {
        Success=0,
        Error=1,
    }
    
    [Opcode(NetOpcode.TestRequest)]
    public class TestRequest: MsgRpcRequest
    {
        public string Name;
        public int Age;
    }

    [Opcode(NetOpcode.TestResponse)]
    public class TestResponse: MsgRpcResponse
    {
        public string Desc;
    }

    [Opcode(NetOpcode.TestChatRequest)]
    public class TestChatRequest: MsgRpcRequest
    {
        public string Content;
    }
    
    [Opcode(NetOpcode.TestChatResponse)]
    public class TestChatResponse: MsgRpcResponse
    {
        
    }
}