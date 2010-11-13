using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Trinity.Encore.Framework.Game
{
    public static class Defines
    {
        public static Version SupportedClientVersion
        {
            get
            {
                Contract.Ensures(Contract.Result<Version>() != null);

                return new Version(4, 0, 1, 13205);
            }
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
