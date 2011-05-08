using System;

namespace Trinity.Encore.Game.Network.Transmission
{
    [Serializable]
    public enum PacketFieldType : byte
    {
        Byte = 0,
        SByte = 1,
        UInt16 = 2,
        Int16 = 3,
        UInt32 = 4,
        Int32 = 5,
        UInt64 = 6,
        Int64 = 7,
        Single = 8,
        Double = 9,
        Decimal = 10,
        Char = 11,
        String = 12,
        Boolean = 13,
        Chars = 14,
        Bytes = 15,
        CString = 16,
        P8String = 17,
        P16String = 18,
        P32String = 19,
        FourCC = 20,
        BigInteger = 21,
        FixedBigInteger = 22,
        PackedUInt64 = 23,
        Guid = 24,
        SmartGuid = 25,
        IPAddress = 26,
    }
}
