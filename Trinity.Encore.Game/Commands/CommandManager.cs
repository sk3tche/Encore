using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using Trinity.Core;
using Trinity.Core.Collections;
using Trinity.Core.Exceptions;
using Trinity.Core.Logging;
using Trinity.Core.Reflection;
using Trinity.Core.Security;
using Trinity.Encore.Game.Security;
using Trinity.Encore.Game.Threading;

namespace Trinity.Encore.Game.Commands
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
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Contract.Assume(asm != null);
                LoadCommands(asm);
            }
        }

        public void AddCommand(Command cmd, params string[] triggers)
        {
            Contract.Requires(cmd != null);
            Contract.Requires(triggers != null);
            Contract.Requires(triggers.Length > 0);

            foreach (var trigger in triggers)
                _commands.Add(trigger, cmd);
        }

        public void RemoveCommand(params string[] triggers)
        {
            Contract.Requires(triggers != null);
            Contract.Requires(triggers.Length > 0);

            foreach (var trigger in triggers)
                _commands.Remove(trigger);
        }

        public Command GetCommand(string trigger)
        {
            Contract.Requires(trigger != null);

            return _commands.TryGet(trigger);
        }

        public IDictionary<string, Command> Commands
        {
            get { return new Dictionary<string, Command>(_commands); }
        }

        public void ExecuteCommand(string[] fullCmd, ICommandUser sender)
        {
            Contract.Requires(fullCmd != null);
            Contract.Requires(fullCmd.Length > 0);

            var cmd = fullCmd.Take(1).Single();

            if (string.IsNullOrWhiteSpace(cmd))
                return;

            var command = GetCommand(cmd);
            if (command == null)
            {
                sender.Respond("Unknown command: {0}".Interpolate(cmd));
                return;
            }

            if (command.RequiresSender && (sender == null || sender is ConsoleCommandUser))
            {
                sender.Respond("Command {0} requires a sender.".Interpolate(cmd));
                return;
            }

            var permission = command.RequiredPermission;
            if (sender != null && permission != null && permission != typeof(ConsolePermission) && !sender.HasPermission(permission))
            {
                sender.Respond("Command {0} requires permission {1}.".Interpolate(cmd, permission));
                return;
            }

            // Process all commands in a serial manner. Not asynchronously, though, as this would cause
            // problems with console cancellation.
            lock (_cmdLock)
            {
                try
                {
                    command.Execute(new CommandArguments(fullCmd.Skip(1)), sender);
                }
                catch (CommandArgumentException ex)
                {
                    sender.Respond("Argument error for command {0}: {1}".Interpolate(cmd, ex.Message));
                }
                catch (Exception ex)
                {
                    ExceptionManager.RegisterException(ex);
                    return;
                }
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
                    throw new ReflectionException("A command class must inherit {0}.".Interpolate(cmdType));

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
