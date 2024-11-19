using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Utilities
{
    public class LogColoring
    {
        public static void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
        public static void LogInline(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void LogInline(string message)
        {
            Console.Write(message);
        }
        public static void LogFormatted(string format, ConsoleColor color, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }
        public static void LogFormatted(string format,params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
