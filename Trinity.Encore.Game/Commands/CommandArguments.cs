using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.Commands
{
    public sealed class CommandArguments
    {
        public CommandArguments(IEnumerable<string> args)
        {
            Contract.Requires(args != null);

            _enum = args.GetEnumerator();
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_enum != null);
        }

        private readonly IEnumerator<string> _enum;

        public bool NextBoolean()
        {
            if (!_enum.MoveNext())
                throw MissingArgument();

            bool value;
            if (bool.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public char NextChar(char? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (char)def;

                throw MissingArgument();
            }

            char value;
            if (char.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public byte NextByte(byte? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (byte)def;

                throw MissingArgument();
            }

            byte value;
            if (byte.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        [CLSCompliant(false)]
        public sbyte NextSByte(sbyte? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (sbyte)def;

                throw MissingArgument();
            }

            if (!_enum.MoveNext())
                throw MissingArgument();

            sbyte value;
            if (sbyte.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        [CLSCompliant(false)]
        public ushort NextUInt16(ushort? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (ushort)def;

                throw MissingArgument();
            }

            ushort value;
            if (ushort.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public short NextInt16(short? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (short)def;

                throw MissingArgument();
            }

            if (!_enum.MoveNext())
                throw MissingArgument();

            short value;
            if (short.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }
        
        [CLSCompliant(false)]
        public uint NextUInt32(uint? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (uint)def;

                throw MissingArgument();
            }

            uint value;
            if (uint.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public int NextInt32(int? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (int)def;

                throw MissingArgument();
            }

            int value;
            if (int.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        [CLSCompliant(false)]
        public ulong NextUInt64(ulong? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (ulong)def;

                throw MissingArgument();
            }

            ulong value;
            if (ulong.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public long NextInt64(long? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (long)def;

                throw MissingArgument();
            }

            long value;
            if (long.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public float NextSingle(float? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (float)def;

                throw MissingArgument();
            }

            float value;
            if (float.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public double NextDouble(double? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (double)def;

                throw MissingArgument();
            }

            double value;
            if (double.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public decimal NextDecimal(decimal? def = null)
        {
            if (!_enum.MoveNext())
            {
                if (def != null)
                    return (decimal)def;

                throw MissingArgument();
            }

            decimal value;
            if (decimal.TryParse(_enum.Current, out value))
                return value;

            throw InvalidFormat();
        }

        public string NextString(string def = null)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (!_enum.MoveNext())
            {
                if (def != null)
                    return def;

                throw MissingArgument();
            }

            var value = _enum.Current;
            Contract.Assume(value != null);
            return value;
        }

        public T NextEnum<T>(T? def = null)
            where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type T is not an enum.");

            T value;
            if (Enum.TryParse(NextString(def != null ? def.ToString() : null), true, out value))
                return value;

            throw InvalidFormat();
        }

        private static CommandArgumentException MissingArgument()
        {
            return new CommandArgumentException("Missing command argument.");
        }

        private static CommandArgumentException InvalidFormat()
        {
            return new CommandArgumentException("Incorrectly formatted argument.");
        }
    }
}
