using System;

namespace Trinity.Encore.AuthenticationService.Authentication
{
    [Serializable]
    public enum AuthenticationResult : byte
    {
        Success = 0x00,
        // 0x1, 0x2, 0xB, 0xD, 0x13-0x15 do not exist, and will result in LOGIN_FAILED
        FailedBanned = 0x03,
        FailedUnknownAccount = 0x04,
        FailedIncorrectPassword = 0x05,
        FailedAlreadyOnline = 0x06,
        FailedNoTime = 0x07,
        FailedDatabaseBusy = 0x08,
        FailedVersionInvalid = 0x09,
        FailedVersionUpdate = 0x0A,
        FailedSuspended = 0x0C,
        SuccessSurvey = 0x0E,
        FailedParentalControls = 0x0F,
        FailedLockedEnforced = 0x10,
        FailedTrialEnded = 0x11,
        FailedUseBattleNet = 0x12,
        FailedTooFast = 0x16,
        FailedChargeback = 0x17,
        FailedGameAccountLocked = 0x18,
        FailedInternetGameRoomWithoutBattleNet = 0x19,
        FailedUnlockableLock = 0x20,
        FailedDisconnected = 0xFF,
    }
}
