using System.Diagnostics.Contracts;
using System.Reflection;

namespace Trinity.Encore.Framework.Core.Configuration
{
    internal sealed class ConfigurationInfo
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Property != null);
            Contract.Invariant(Attribute != null);
        }

        public ConfigurationInfo(PropertyInfo prop, ConfigurationVariableAttribute attr)
        {
            Contract.Requires(prop != null);
            Contract.Requires(attr != null);

            Property = prop;
            Attribute = attr;
        }

        public object GetValue()
        {
            return Property.GetValue(null, null);
        }

        public void SetValue(object value)
        {
            Property.SetValue(null, value, null);
        }

        public PropertyInfo Property { get; private set; }

        public ConfigurationVariableAttribute Attribute { get; private set; }
    }
}
