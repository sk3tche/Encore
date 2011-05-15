using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Trinity.Encore.Game
{
    public static class Constants
    {
        public static class Accounts
        {
            public const int MinNameLength = 3;

            public const int MaxNameLength = 20;

            public const int MinPasswordLength = 3;

            public const int MaxPasswordLength = 16;
        }

        public static class Game
        {
            public static Version SupportedClientVersion
            {
                get
                {
                    Contract.Ensures(Contract.Result<Version>() != null);

                    return new Version(4, 1, 0, 13850);
                }
            }

            public const int MaxLevelCap = 255;
        }

        public static class Protocol
        {
            public static Encoding Encoding
            {
                get
                {
                    Contract.Ensures(Contract.Result<Encoding>() != null);

                    return Encoding.UTF8;
                }
            }

            public const int LargePacketThreshold = 0x7fff;
        }
    }
}
