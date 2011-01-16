using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Trinity.Core.Logging.Loggers
{
    /// <summary>
    /// A logger that outputs text to the console.
    /// </summary>
    internal sealed class ConsoleLogger : ILogger
    {
        private readonly StringBuilder _builder = new StringBuilder();

        private static ConsoleColor? GetColor(ConsoleColor color)
        {
            if (LogManager.UseConsoleColors)
                return color;

            return null;
        }

        public void WriteInformation(string logString)
        {
            WriteToConsole(logString, GetColor(ConsoleColor.Green));
        }

        public void WriteWarning(string logString)
        {
            WriteToConsole(logString, GetColor(ConsoleColor.Yellow));
        }

        public void WriteError(string logString)
        {
            WriteToConsole(logString, GetColor(ConsoleColor.Red));
        }

        private void WriteToConsole(string str, ConsoleColor? color)
        {
            Contract.Requires(str != null);

            if (color != null)
                Console.ForegroundColor = color.Value;

            if (LogManager.UseConsoleTimestamp)
                _builder.Append("[").Append(DateTime.Now).Append("] ");

            _builder.Append(str);
            Console.WriteLine(_builder.ToString());
            _builder.Clear();

            if (color != null)
                Console.ResetColor();
        }
    }
}
