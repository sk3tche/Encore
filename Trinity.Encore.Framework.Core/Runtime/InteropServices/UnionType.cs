using System;
using System.Runtime.InteropServices;

namespace Trinity.Encore.Framework.Core.Runtime.InteropServices
{
    /// <summary>
    /// A structure which can represent multiple data types simultaneously.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct UnionType : IComparable<UnionType>, IEquatable<UnionType>
    {
        [FieldOffset(0)]
        public byte Byte;

        [FieldOffset(0)]
        [CLSCompliant(false)]
        public sbyte SByte;

        [FieldOffset(0)]
        [CLSCompliant(false)]
        public ushort UInt16;

        [FieldOffset(0)]
        public short Int16;

        [FieldOffset(0)]
        [CLSCompliant(false)]
        public uint UInt32;

        [FieldOffset(0)]
        public int Int32;

        [FieldOffset(0)]
        [CLSCompliant(false)]
        public ulong UInt64;

        [FieldOffset(0)]
        public long Int64;

        [FieldOffset(0)]
        public char Char;

        [FieldOffset(0)]
        public float Single;

        [FieldOffset(0)]
        public double Double;

        [FieldOffset(0)]
        public decimal Decimal;

        public int CompareTo(UnionType other)
        {
            if (this > other)
                return 1;

            if (this < other)
                return -1;

            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is UnionType))
                return false;

            return Equals((UnionType)obj);
        }

        public override int GetHashCode()
        {
            return Utilities.GetHashCode(Byte, SByte, UInt16, Int16, UInt32, Int32, UInt64, Int64, Char, Single, Double, Decimal);
        }

        public bool Equals(UnionType other)
        {
            return Decimal == other.Decimal;
        }

        public static bool operator ==(UnionType a, UnionType b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(UnionType a, UnionType b)
        {
            return !(a == b);
        }

        public static bool operator >(UnionType a, UnionType b)
        {
            return a.Decimal > b.Decimal;
        }

        public static bool operator <(UnionType a, UnionType b)
        {
            return a.Decimal < b.Decimal;
        }
    }
}
