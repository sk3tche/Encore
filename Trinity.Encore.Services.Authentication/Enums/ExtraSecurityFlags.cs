using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trinity.Encore.Services.Authentication.Enums
{
    [Flags]
    public enum ExtraSecurityFlags : byte
    {
        PIN = 0x01,
        Matrix = 0x02,
        SecurityToken = 0x04
    }
}
