using System;

namespace SNet
{
    public enum AsyncLogColor {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }
    public class LogHelper
    {
        public static Action<string> LogFunc;
        public static Action<AsyncLogColor, string> ColorLogFunc;
        public static Action<string> WarnFunc;
        public static Action<string> ErrorFunc;
        public static void Log(string msg, params object[] args) {
            msg = string.Format(msg, args);
            if(LogFunc != null) {
                LogFunc(msg);
            }
            else {
                Console.WriteLine(msg);
            }
        }
        public static void ColorLog(AsyncLogColor color, string msg, params object[] args) {
            msg = string.Format(msg, args);
            if(ColorLogFunc != null) {
                ColorLogFunc(color, msg);
            }
            else {
                ConsoleLog(msg, color);
            }
        }
        public static void Warn(string msg, params object[] args) {
            msg = string.Format(msg, args);
            if(WarnFunc != null) {
                WarnFunc(msg);
            }
            else {
                ConsoleLog(msg, AsyncLogColor.Yellow);
            }
        }
        public static void Error(string msg, params object[] args) {
            msg = string.Format(msg, args);
            if(ErrorFunc != null) {
                ErrorFunc(msg);
            }
            else {
                ConsoleLog(msg, AsyncLogColor.Red);
            }
        }
        static void ConsoleLog(string msg, AsyncLogColor color) {
            switch(color) {
                case AsyncLogColor.Red:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case AsyncLogColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case AsyncLogColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case AsyncLogColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case AsyncLogColor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case AsyncLogColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case AsyncLogColor.None:
                default:
                    Console.WriteLine(msg);
                    break;
            }
        }
    }
}