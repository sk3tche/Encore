using Trinity.Encore.Framework.Core.Threading.Actors;
using Trinity.Encore.Framework.Game.Commands;

namespace Trinity.Encore.Services.Authentication
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
