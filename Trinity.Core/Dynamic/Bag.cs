using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using Trinity.Core.Collections;

namespace Trinity.Core.Dynamic
{
    /// <summary>
    /// Represents a data bag to which properties can be dynamically added/retrieved/removed at runtime.
    /// </summary>
    [Serializable]
    public sealed class Bag : DynamicObject
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_values != null);
        }

        /// <summary>
        /// Gets a value representing the amount of set properties.
        /// </summary>
        public int Count
        {
            get { return _values.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether or not any properties are set.
        /// </summary>
        public bool HasProperties
        {
            get { return Count > 0; }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = binder.Name;
            if (_values.ContainsKey(name))
                return false;

            _values[name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            Contract.Assume(name != null);
            result = _values.TryGet(name);

            return result != null;
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            var name = binder.Name;
            if (!_values.ContainsKey(name))
                return false;

            _values.Remove(name);
            return true;
        }
    }
}
