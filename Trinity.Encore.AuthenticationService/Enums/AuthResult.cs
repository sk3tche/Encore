namespace Trinity.Encore.AuthenticationService.Enums
{
    public enum AuthResult : byte
    {
        Success = 0x00,
        // 0x1, 0x2, 0xB, 0xD, 0x13-0x15 do not exist, and will result in LOGIN_FAILED
        FailBanned = 0x03,
        FailUnknownAccount = 0x04,
        FailIncorrectPassword = 0x05,
        FailAlreadyOnline = 0x06,
        FailNoTime = 0x07,
        FailDBBusy = 0x08,
        FailVersionInvalid = 0x09,
        FaiLVersionUpdate = 0x0A,
        FailSuspended = 0x0C,
        SuccessSurvey = 0x0E,
        FailParentalControls = 0x0F,
        FailLockedEnforced = 0x10,
        FailTrialEnded = 0x11,
        FailUseBattlenet = 0x12,
        FailTooFast = 0x16,
        FailChargeback = 0x17,
        FailGameAccountLocked = 0x18,
        FailInternetGameRoomWithoutBNet = 0x19,
        FailUnlockableLock = 0x20,
        FailDisconnected = 0xFF,
    }
}
