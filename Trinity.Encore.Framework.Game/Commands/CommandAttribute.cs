using System;
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
            Contract.Invariant(Triggers.Length >= 1);
        }

        public CommandAttribute(params string[] triggers)
        {
            Contract.Requires(triggers != null);
            Contract.Requires(triggers.Length >= 1);

            Triggers = triggers;
        }

        public string[] Triggers { get; private set; }

        public string Description { get; set; }
    }
}
