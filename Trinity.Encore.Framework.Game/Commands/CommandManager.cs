using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Reflection;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Game.Threading;

namespace Trinity.Encore.Framework.Game.Commands
{
    public sealed class CommandManager : SingletonActor<CommandManager>
    {
        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase);

        private readonly object _cmdLock = new object();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_commands != null);
            Contract.Invariant(_cmdLock != null);
        }

        private CommandManager()
        {
            LoadAllCommands();
        }

        public void AddCommand(Command cmd, params string[] triggers)
        {
            Contract.Requires(cmd != null);
            Contract.Requires(triggers != null);
            Contract.Requires(triggers.Length > 0);

            lock (_commands)
                foreach (var trigger in triggers)
                    _commands.Add(trigger, cmd);
        }

        public void RemoveCommand(params string[] triggers)
        {
            Contract.Requires(triggers != null);
            Contract.Requires(triggers.Length > 0);

            lock (_commands)
                foreach (var trigger in triggers)
                    _commands.Remove(trigger);
        }

        public Command GetCommand(string trigger)
        {
            Contract.Requires(trigger != null);

            lock (_commands)
                return _commands.TryGet(trigger);
        }

        public IDictionary<string, Command> GetCommands()
        {
            lock (_commands)
                return new Dictionary<string, Command>(_commands); // Cloning is the future!
        }

        public void ExecuteCommand(string[] fullCmd, IPermissible sender)
        {
            Contract.Requires(fullCmd != null);
            Contract.Requires(fullCmd.Length > 0);

            var cmd = fullCmd.Take(1).Single();
            var args = fullCmd.Skip(1);

            Contract.Assume(cmd != null);
            var command = GetCommand(cmd);
            if (command == null)
                return; // TODO: Log.

            if (command.RequiresSender && sender == null)
                return; // TODO: Log.

            var permission = command.RequiredPermission;
            if (permission != null && sender == null)
                return; // TODO: Log.

            if (sender != null && permission != null && !sender.HasPermission(permission))
                return; // TODO: Log.

            // Process all commands in a serial manner. Not asynchronously, though, as this would cause
            // problems with console cancellation.
            lock (_cmdLock)
            {
                try
                {
                    var correctArgs = command.Execute(new CommandArguments(args), sender);
                }
                catch (Exception ex)
                {
                    ExceptionManager.RegisterException(ex);
                }
            }
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

            var cmdType = typeof(Command);

            foreach (var type in asm.GetTypes())
            {
                Contract.Assume(type != null);

                var attr = type.GetCustomAttribute<CommandAttribute>();
                if (attr == null)
                    continue;

                if (!type.IsAssignableTo(cmdType))
                    throw new ReflectionException(string.Format("A command class must inherit {0}.", cmdType));

                if (type.IsGenericType)
                    throw new ReflectionException("A command class cannot be generic.");

                if (type.IsAbstract)
                    throw new ReflectionException("A command class cannot be abstract.");

                var ctor = type.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0);
                if (ctor == null)
                    throw new ReflectionException("A command class must have a public parameterless constructor.");

                var cmd = (Command)ctor.Invoke(null);
                Contract.Assume(cmd != null);
                AddCommand(cmd, attr.Triggers);
            }
        }
    }
}
