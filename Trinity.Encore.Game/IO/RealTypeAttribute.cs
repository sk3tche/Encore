using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO
{
    /// <summary>
    /// Instructs ClientDbReader to treat a property as a different type than the type
    /// it is declared as.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class RealTypeAttribute : Attribute
    {
        public RealTypeAttribute(Type type)
        {
            Contract.Requires(type != null);

            RealType = type;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(RealType != null);
        }

        public Type RealType { get; private set; }
    }
}
