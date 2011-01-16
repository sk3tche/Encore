using Trinity.Core.Threading.Actors;
using Trinity.Encore.Game.Commands;

namespace Trinity.Encore.AccountService
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
        }
    }
}
