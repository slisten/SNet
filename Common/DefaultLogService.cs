using System;

namespace Common
{
    public class DefaultLogService: ILog
    {
        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        public void LogError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        
        public void LogWarning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void OnInit()
        {
            
        }

        public void OnDestroy()
        {
            
        }
    }
}