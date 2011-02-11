using System;

namespace Trinity.Encore.AuthenticationService.Authentication
{
    [Flags]
    [Serializable]
    public enum ExtraSecurityFlags : byte
    {
        None = 0x00,
        Pin = 0x01,
        Matrix = 0x02,
        SecurityToken = 0x04,
    }
}
