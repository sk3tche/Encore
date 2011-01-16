using Trinity.Core.Threading.Actors;
using Trinity.Encore.Game.Commands;

namespace Trinity.Encore.AuthenticationService
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (ActorContext.Global)
            {
                AuthenticationApplication.Instance.Start(args);
                CommandConsole.Run();
                AuthenticationApplication.Instance.Stop();
            }
        }
    }
}
