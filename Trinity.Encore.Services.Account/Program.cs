using System;
using Trinity.Encore.Framework.Core.Threading.Actors;
using Trinity.Encore.Framework.Game.Commands;

namespace Trinity.Encore.Services.Account
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (ActorContext.Global)
            {
                AccountApplication.Instance.Start(args);
                CommandConsole.Run();
                AccountApplication.Instance.Stop();
            }

            Console.ReadKey();
        }
    }
}
