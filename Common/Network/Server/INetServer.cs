namespace Common.Server
{
    public interface INetServer: IService
    {
        void Listen(string ip, int port);
    }
}