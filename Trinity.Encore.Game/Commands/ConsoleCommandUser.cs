using System;
using System.Text;
using Trinity.Core.Security;

namespace Trinity.Encore.Game.Commands
{
    internal sealed class ConsoleCommandUser : RestrictedObject, ICommandUser
    {
        public void Respond(string response)
        {
            Console.WriteLine(response);
        }
    }
}
