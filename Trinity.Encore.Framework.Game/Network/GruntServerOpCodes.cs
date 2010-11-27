using System;

namespace Trinity.Encore.Framework.Game.Network
{
    [Serializable]
    public enum GruntServerOpCodes : byte
    {
        // Authentication:
        /// <summary>
        /// CMD_GRUNT_AUTH_CHALLENGE.
        /// </summary>
        AuthenticationChallenge = 0x00,
        AuthenticationProof = 0x01,
        /// <summary>
        /// CMD_GRUNT_AUTH_VERIFY.
        /// </summary>
        AuthenticationVerification = 0x02,

        // Connection:
        /// <summary>
        /// CMD_GRUNT_CONN_PING.
        /// </summary>
        ConnectionPing = 0x10,
        /// <summary>
        /// CMD_GRUNT_CONN_PONG.
        /// </summary>
        ConnectionPong = 0x11,

        // IPC:
        /// <summary>
        /// CMD_GRUNT_HELLO.
        /// 
        /// Not needed in Encore.
        /// </summary>
        Hello = 0x20,
        /// <summary>
        /// CMD_GRUNT_PROVESESSION.
        /// 
        /// Not needed in Encore.
        /// </summary>
        ProveSession = 0x21,
        /// <summary>
        /// CMD_GRUNT_KICK.
        /// 
        /// Not needed in Encore.
        /// </summary>
        Kick = 0x24,
        /// <summary>
        /// CMD_GRUNT_PCWARNING.
        /// 
        /// Not needed in Encore.
        /// </summary>
        PCWarning = 0x29,
        /// <summary>
        /// CMD_GRUNT_STRINGS.
        /// 
        /// Not needed in Encore.
        /// </summary>
        Strings = 0x41,
        /// <summary>
        /// CMD_GRUNT_SUNKENUPDATE.
        /// 
        /// Not needed in Encore.
        /// </summary>
        SunkenUpdate = 0x44,
        /// <summary>
        /// CMD_GRUNT_SUNKEN_ONLINE.
        /// 
        /// Not needed in Encore.
        /// </summary>
        SunkenOnline = 0x46,
    }
}
