using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trinity.Encore.Services.Authentication.Enums
{
    public enum AuthResult : byte
    {
        Success = 0x00,
        FailUnknown0 = 0x01,
        FailUnknown1 = 0x02,
        FailBanned = 0x03,
        FailUnknownAccount = 0x04,
        FailIncorrectPassword = 0x05,
        FailAlreadyOnline = 0x06,
        FailNoTime = 0x07,
        FailDBBusy = 0x08,
        FailVersionInvalid = 0x09,
        FaiLVersionUpdate = 0x0A,
        FailInvalidServer = 0x0B,
        FailSuspended = 0x0C,
        FailNoAccess = 0x0D,
        SuccessSurvey = 0x0E,
        FailParentalControls = 0x0F,
        FailLockedEnforced = 0x10,
        FailTrialEnded = 0x11,
        FailUseBattlenet = 0x12
    }
}
