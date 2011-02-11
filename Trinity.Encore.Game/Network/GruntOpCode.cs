using System;

namespace Trinity.Encore.Game.Network
{
    [Serializable]
    public enum GruntOpCode : byte
    {
        #region Authentication

        /// <summary>
        /// CMD_AUTH_LOGON_CHALLENGE.
        /// </summary>
        AuthenticationLogOnChallenge = 0x00,
        /// <summary>
        /// CMD_AUTH_LOGON_PROOF.
        /// </summary>
        AuthenticationLogOnProof = 0x01,
        /// <summary>
        /// CMD_AUTH_RECONNECT_CHALLENGE.
        /// </summary>
        AuthenticationReconnectChallenge = 0x02,
        /// <summary>
        /// CMD_AUTH_RECONNECT_PROOF.
        /// </summary>
        AuthenticationReconnectProof = 0x03,

        #endregion

        #region Realms

        /// <summary>
        /// CMD_REALM_LIST.
        /// </summary>
        RealmList = 0x10,

        #endregion

        #region Patching

        /// <summary>
        /// CMD_XFER_INITIATE.
        /// </summary>
        TransferInitiate = 0x30,
        /// <summary>
        /// CMD_XFER_DATA.
        /// </summary>
        TransferData = 0x31,
        /// <summary>
        /// CMD_XFER_COMPLETE.
        /// </summary>
        TransferComplete = 0x32,
        /// <summary>
        /// CMD_XFER_RESUME.
        /// </summary>
        TransferResume = 0x33,
        /// <summary>
        /// CMD_XFER_CANCEL.
        /// </summary>
        TransferCancel = 0x34,

        #endregion
    }
}
