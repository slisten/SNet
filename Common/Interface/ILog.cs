namespace Common
{
    public interface ILog: IService
    {
        void Log(string msg);
        void LogError(string msg);
        void LogWarning(string msg);
    }
}