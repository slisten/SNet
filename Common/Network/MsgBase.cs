namespace Common
{
    public class MsgBase
    {
        
    }

    public class MsgRequest: MsgBase
    {
        
    }

    public class MsgResponse: MsgBase
    {
        public ErrorCode Result;
    }

    public class MsgRpcRequest: MsgRequest
    {
        public int RpcID;
    }

    public class MsgRpcResponse: MsgResponse
    {
        public int RpcID;
    }
}