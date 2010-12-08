using System;

namespace Trinity.Encore.Services.Authentication.Enums
{
    [Flags]
    public enum ExtraSecurityFlags : byte
    {
        None = 0,
        PIN = 0x01,
        Matrix = 0x02,
        SecurityToken = 0x04
    }
}
