using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace Trinity.Core
{
    public static class Utility
    {
        public static bool IsRunningMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static string ToCamelCase(string input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);

            const char space = ' ';
            var newName = new StringBuilder();
            var upperCase = true;

            foreach (var chr in input)
            {
                var c = chr;

                if (c == '_')
                    c = space;

                c = upperCase ? char.ToUpper(c, CultureInfo.InvariantCulture) : char.ToLower(c, CultureInfo.InvariantCulture);

                if (c == space)
                {
                    upperCase = true;
                    continue;
                }

                upperCase = false;

                newName.Append(c);
            }

            return newName.ToString();
        }

        public static byte[] HexStringToBinary(string data)
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Length % 2 == 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var bytes = new List<byte>();

            for (var i = 0; i < data.Length; i += 2)
            {
                Contract.Assume(i + 2 <= data.Length);
                bytes.Add(byte.Parse(data.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
            }

            return bytes.ToArray();
        }

        public static string BinaryToHexString(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var str = string.Empty;

            for (var i = 0; i < data.Length; ++i)
                str += data[i].ToString("X2", CultureInfo.InvariantCulture);

            return str;
        }
    }
}
