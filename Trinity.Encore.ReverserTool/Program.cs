using System;
using Trinity.Core.Threading.Actors;
using Trinity.Encore.Game.Commands;

namespace Trinity.Encore.ReverserTool
{
    internal static class Program
    {
        private static void Main()
        {
            using (ActorContext.Global)
            {
                Console.WriteLine("Welcome to the Encore ReverserTool. Type '?' for help.");
                CommandConsole.Run();
            }
        }
    }
}
