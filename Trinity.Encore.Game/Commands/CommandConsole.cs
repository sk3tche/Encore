using System;

namespace Trinity.Encore.Game.Commands
{
    public static class CommandConsole
    {
        private static readonly ConsoleCommandUser _user = new ConsoleCommandUser();

        public static bool StopConsole { get; set; }

        public static void Run()
        {
            while (!StopConsole)
            {
                var line = Console.ReadLine();
                if (line == null)
                    break;

                CommandManager.Instance.ExecuteCommand(line.Split(' '), _user);
            }
        }
    }
}
