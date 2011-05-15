using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.Network.Transmission
{
    /// <summary>
    /// Represents a field in a packet's structure.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    [SuppressMessage("Microsoft.Performance", "CA1815", Justification = "This type is not equatable.")]
    public struct PacketField<T>
    {
        /// <summary>
        /// The protocol type of the field.
        /// </summary>
        public readonly PacketFieldType Type;

        /// <summary>
        /// The value of the field.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104", Justification = "This field's value object is meant to be mutable.")]
        public readonly T Value;

        /// <summary>
        /// The name of the field
        /// </summary>
        public readonly string Name;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Name != null);
        }

        public PacketField(PacketFieldType type, T value, string name)
        {
            Contract.Requires(name != null);
            Contract.Ensures(Contract.ValueAtReturn(out this).Type == type);
            Contract.Ensures(ReferenceEquals(Contract.ValueAtReturn(out this).Value, value));
            Contract.Ensures(Contract.ValueAtReturn(out this).Name == name);

            Type = type;
            Value = value;
            Name = name;
        }

        public static implicit operator T(PacketField<T> field)
        {
            Contract.Ensures(ReferenceEquals(Contract.Result<T>(), field.Value));

            return field.Value;
        }
    }
}
