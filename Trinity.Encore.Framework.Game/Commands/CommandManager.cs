using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Trinity.Encore.Framework.Game.Threading;

namespace Trinity.Encore.Framework.Game.Commands
{
    public sealed class CommandManager : SingletonActor<CommandManager>
    {
        private CommandManager()
        {
        }

        private void LoadAllCommands()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Contract.Assume(asm != null);
                LoadCommands(asm);
            }
        }

        public void LoadCommands(Assembly asm)
        {
            Contract.Requires(asm != null);
        }
    }
}
