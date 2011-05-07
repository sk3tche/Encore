using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.ReverserTool
{
    public static class OpCodeUtility
    {
        // Values in sync with 4.1.0.13850.

        public const int MaxOpCode = ushort.MaxValue;

        public static bool IsCompressableOpCode(int opCode)
        {
            Contract.Requires(opCode > 0);

            return (opCode & 0x4c9) == 0x440;
        }

        public static int? CompressOpCode(int opCode)
        {
            Contract.Requires(opCode > 0);

            if (IsCompressableOpCode(opCode))
                return (((opCode & 0xf800) >> 5) | ((opCode & 0x300) >> 4) | ((opCode & 0x6) >> 1) | ((opCode & 0x30) >> 2));

            return null;
        }

        public static IEnumerable<int> GetOpCodesForCondensedOpCode(int condensedOpCode)
        {
            for (var i = 1; i < MaxOpCode; i++)
                if ((i & 0x4C09) != 0x440)
                    if (CompressOpCode(i) == condensedOpCode)
                        yield return i;
        }

        public static bool IsJamClientOpCode(int opCode)
        {
            Contract.Requires(opCode > 0);

            return (opCode & 0x467c) == 0x608;
        }

        public static int? GetJamClientOpCode(int opCode)
        {
            Contract.Requires(opCode > 0);

            if (IsJamClientOpCode(opCode))
                return ((opCode & 0x8000) >> 8) | opCode & 3 | ((opCode & 0x3800) >> 7) | ((opCode & 0x180) >> 5);

            return null;
        }

        public static bool IsJamClientConnectionOpCode(int opCode)
        {
            Contract.Requires(opCode > 0);

            return (opCode & 0xfcfB) == 0x4c0;
        }

        public static int? GetJamClientConnectionOpCode(int opCode)
        {
            Contract.Requires(opCode > 0);

            if (IsJamClientConnectionOpCode(opCode))
                return ((opCode & 0x4) >> 2) | ((opCode & 0x300) >> 7);

            return null;
        }
    }
}
