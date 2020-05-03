using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MUDhub.Server
{
    public static class StartupLogger
    {
        public static bool LogMessage(string configPath, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                InnerLogMessage(configPath,value);
                return true;
            }
            return false;
        }

        public static bool LogMessage(string configPath, object? value)
        {
            if (value is null)
            {
                InnerLogMessage(configPath,value);
                return true;
            }
            return false;
        }


        private static void InnerLogMessage(string configPath, object? value)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("- Error in Configuration:");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"'{configPath}'");
            Console.ResetColor();
            Console.Write($" has no valid value!");
            Console.WriteLine();
        }
    }
}
