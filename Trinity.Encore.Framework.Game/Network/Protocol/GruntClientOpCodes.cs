namespace Trinity.Encore.Framework.Game.Network.Protocol
{
    public enum GruntClientOpCodes : byte
    {
        // Authentication:
        /// <summary>
        /// CMD_AUTH_LOGON_CHALLENGE.
        /// </summary>
        AuthenticationLogonChallenge = 0x00,
        /// <summary>
        /// CMD_AUTH_LOGON_PROOF.
        /// </summary>
        AuthenticationLogonProof = 0x01,
        /// <summary>
        /// CMD_AUTH_RECONNECT_CHALLENGE.
        /// </summary>
        AuthenticationReconnectChallenge = 0x02,
        /// <summary>
        /// CMD_AUTH_RECONNECT_PROOF.
        /// </summary>
        AuthenticationReconnectProof = 0x03,

        // Realms:
        /// <summary>
        /// CMD_REALM_LIST.
        /// </summary>
        RealmList = 0x10,

        // Patching:
        /// <summary>
        /// CMD_XFER_INITIATE.
        /// 
        /// Deprecated.
        /// </summary>
        TransferInitiate = 0x30,
        /// <summary>
        /// CMD_XFER_DATA.
        /// 
        /// Deprecated.
        /// </summary>
        TransferData = 0x31,
    }
}
