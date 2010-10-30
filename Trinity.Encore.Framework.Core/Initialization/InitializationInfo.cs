using System.Diagnostics.Contracts;
using System.Reflection;

namespace Trinity.Encore.Framework.Core.Initialization
{
    internal sealed class InitializationInfo
    {
        public InitializationInfo(InitializableAttribute attr, MethodInfo method)
        {
            Contract.Requires(attr != null);
            Contract.Requires(method != null);

            Attribute = attr;
            Method = method;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Attribute != null);
            Contract.Invariant(Method != null);
        }

        public InitializableAttribute Attribute { get; private set; }

        public MethodInfo Method { get; private set; }

        public bool Initialized { get; set; }
    }
}
