using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Core.IO;

namespace Trinity.Encore.Game.IO.Compression
{
    // Based on code by Ladislav Zezula and Foole.
    // TODO: Figure out if this is reusable enough to be moved to Trinity.Core.
    public static class ImaAdpcmDecompressor
    {
        private static readonly int[] _sLookup1 =
        {
            0x0007, 0x0008, 0x0009, 0x000a, 0x000b, 0x000c, 0x000d, 0x000e,
            0x0010, 0x0011, 0x0013, 0x0015, 0x0017, 0x0019, 0x001c, 0x001f,
            0x0022, 0x0025, 0x0029, 0x002d, 0x0032, 0x0037, 0x003c, 0x0042,
            0x0049, 0x0050, 0x0058, 0x0061, 0x006b, 0x0076, 0x0082, 0x008f,
            0x009d, 0x00ad, 0x00Be, 0x00d1, 0x00e6, 0x00Fd, 0x0117, 0x0133,
            0x0151, 0x0173, 0x0198, 0x01c1, 0x01ee, 0x0220, 0x0256, 0x0292,
            0x02d4, 0x031c, 0x036c, 0x03c3, 0x0424, 0x048e, 0x0502, 0x0583,
            0x0610, 0x06ab, 0x0756, 0x0812, 0x08e0, 0x09c3, 0x0abd, 0x0bd0,
            0x0cff, 0x0e4c, 0x0fba, 0x114c, 0x1307, 0x14ee, 0x1706, 0x1954,
            0x1bdc, 0x1ea5, 0x21b6, 0x2515, 0x28ca, 0x2cdf, 0x315b, 0x364b,
            0x3bb9, 0x41b2, 0x4844, 0x4f7e, 0x5771, 0x602f, 0x69Ce, 0x7462,
            0x7fff,
        };

        private static readonly int[] _sLookup2 =
        {
            -1, 0, -1, 4, -1, 2, -1, 6,
            -1, 1, -1, 5, -1, 3, -1, 7,
            -1, 1, -1, 5, -1, 3, -1, 7,
            -1, 2, -1, 4, -1, 6, -1, 8,
        };

        public static byte[] Decompress(BinaryReader input, int channelCount)
        {
            Contract.Requires(input != null);
            Contract.Requires(channelCount >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var array1 = new[] { 0x2c, 0x2c };
            var array2 = new int[channelCount];

            var outputStream = new MemoryStream();
            using (var output = new BinaryWriter(outputStream))
            {
                input.ReadByte();
                var shift = input.ReadByte();

                for (var i = 0; i < channelCount; i++)
                {
                    var temp = input.ReadInt16();
                    array2[i] = temp;
                    output.Write(temp);
                }

                var channel = channelCount - 1;

                while (!input.BaseStream.IsRead())
                {
                    var value = input.ReadByte();

                    if (channelCount == 2)
                        channel = 1 - channel;

                    if ((value & 0x80) != 0)
                    {
                        switch (value & 0x7f)
                        {
                            case 0:
                                if (array1[channel] != 0)
                                    array1[channel]--;

                                output.Write((short)array2[channel]);
                                break;
                            case 1:
                                array1[channel] += 8;

                                if (array1[channel] > 0x58)
                                    array1[channel] = 0x58;

                                if (channelCount == 2)
                                    channel = 1 - channel;
                                break;
                            case 2:
                                break;
                            default:
                                array1[channel] -= 8;
                                if (array1[channel] < 0)
                                    array1[channel] = 0;

                                if (channelCount == 2)
                                    channel = 1 - channel;
                                break;
                        }
                    }
                    else
                    {
                        var temp1 = _sLookup1[array1[channel]];
                        var temp2 = temp1 >> shift;

                        if ((value & 1) != 0)
                            temp2 += (temp1 >> 0);

                        if ((value & 2) != 0)
                            temp2 += (temp1 >> 1);

                        if ((value & 4) != 0)
                            temp2 += (temp1 >> 2);

                        if ((value & 8) != 0)
                            temp2 += (temp1 >> 3);

                        if ((value & 0x10) != 0)
                            temp2 += (temp1 >> 4);

                        if ((value & 0x20) != 0)
                            temp2 += (temp1 >> 5);

                        var temp3 = array2[channel];

                        if ((value & 0x40) != 0)
                        {
                            temp3 -= temp2;
                            if (temp3 <= short.MinValue)
                                temp3 = short.MinValue;
                        }
                        else
                        {
                            temp3 += temp2;
                            if (temp3 >= short.MaxValue)
                                temp3 = short.MaxValue;
                        }

                        array2[channel] = temp3;
                        output.Write((short)temp3);

                        array1[channel] += _sLookup2[value & 0x1f];

                        if (array1[channel] < 0)
                            array1[channel] = 0;
                        else if (array1[channel] > 0x58)
                            array1[channel] = 0x58;
                    }
                }

                var arr = outputStream.ToArray();
                Contract.Assume(arr != null);
                return arr;
            }
        }
    }
}
