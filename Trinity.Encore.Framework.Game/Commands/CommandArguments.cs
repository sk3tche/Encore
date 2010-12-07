using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Trinity.Encore.Framework.Game.Commands
{
    public sealed class CommandArguments
    {
        public CommandArguments(IEnumerable<string> args)
        {
            Contract.Requires(args != null);

            _args = args;
            _enum = _args.GetEnumerator();
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_args != null);
            Contract.Invariant(_enum != null);
        }

        private readonly IEnumerator<string> _enum;

        private readonly IEnumerable<string> _args;

        public bool? NextBoolean()
        {
            if (!_enum.MoveNext())
                return null;

            bool value;
            if (bool.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public char? NextChar()
        {
            if (!_enum.MoveNext())
                return null;

            char value;
            if (char.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public byte? NextByte()
        {
            if (!_enum.MoveNext())
                return null;

            byte value;
            if (byte.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public sbyte? NextSByte()
        {
            if (!_enum.MoveNext())
                return null;

            sbyte value;
            if (sbyte.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public ushort? NextUInt16()
        {
            if (!_enum.MoveNext())
                return null;

            ushort value;
            if (ushort.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public short? NextInt16()
        {
            if (!_enum.MoveNext())
                return null;

            short value;
            if (short.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public uint? NextUInt32()
        {
            if (!_enum.MoveNext())
                return null;

            uint value;
            if (uint.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public int? NextInt32()
        {
            if (!_enum.MoveNext())
                return null;

            int value;
            if (int.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public ulong? NextUInt64()
        {
            if (!_enum.MoveNext())
                return null;

            ulong value;
            if (ulong.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public long? NextInt64()
        {
            if (!_enum.MoveNext())
                return null;

            long value;
            if (long.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public float? NextSingle()
        {
            if (!_enum.MoveNext())
                return null;

            float value;
            if (float.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public double? NextDouble()
        {
            if (!_enum.MoveNext())
                return null;

            double value;
            if (double.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public decimal? NextDecimal()
        {
            if (!_enum.MoveNext())
                return null;

            decimal value;
            if (decimal.TryParse(_enum.Current, out value))
                return value;

            return null;
        }

        public string NextString()
        {
            return _enum.MoveNext() ? _enum.Current : null;
        }
    }
}
