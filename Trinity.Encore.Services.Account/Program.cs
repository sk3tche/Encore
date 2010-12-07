using System;
using Trinity.Encore.Framework.Game.Commands;

namespace Trinity.Encore.Services.Account
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            AccountApplication.Instance.Start(args);
            CommandConsole.Run();
            AccountApplication.Instance.Stop();
        }
    }
}
