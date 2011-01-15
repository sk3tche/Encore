using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.Commands
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandAttribute : Attribute
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Triggers != null);
            Contract.Invariant(Triggers.Length > 0);
        }

        public CommandAttribute(params string[] triggers)
        {
            Contract.Requires(triggers != null);
            Contract.Requires(triggers.Length > 0);

            Triggers = triggers;
        }

        public string[] Triggers { get; private set; }
    }
}
