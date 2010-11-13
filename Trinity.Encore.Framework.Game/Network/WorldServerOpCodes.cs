using System;

namespace Trinity.Encore.Framework.Game.Network
{
    /// <summary>
    /// A list of packet opcodes gleaned from client version 4.0.1.13205.
    /// </summary>
    [Serializable]
    public enum WorldServerOpCodes : uint
    {
        // Authentication:
        /// <summary>
        /// SMSG_AUTH_CHALLENGE.
        /// </summary>
        ServerAuthenticationChallenge = 0x8500,
        /// <summary>
        /// CMSG_AUTH_SESSION.
        /// </summary>
        ClientAuthenticationData = 0x3000,
        /// <summary>
        /// SMSG_AUTH_RESPONSE.
        /// </summary>
        ServerAuthenticationResult = 0xEB58,

        // Session:
        /// <summary>
        /// SMSG_LOGOUT_RESPONSE
        /// </summary>
        ServerSessionLogoutResult = 0x63BC,
        /// <summary>
        /// SMSG_LOGOUT_COMPLETE.
        /// </summary>
        ServerSessionLogoutComplete = 0x8311,
        /// <summary>
        /// SMSG_LOGOUT_CANCEL_ACK.
        /// </summary>
        ServerSessionLogoutCancelAck = 0xA395,
        /// <summary>
        /// SMSG_NEW_WORLD.
        /// </summary>
        ServerSessionNewWorld = 0x4A5D,

        // Connection:
        /// <summary>
        /// CMSG_PING.
        /// </summary>
        ServerConnectionPing = 0x1001,
        /// <summary>
        /// CMSG_REDIRECT_ERROR.
        /// </summary>
        ServerConnectionRedirectError = 0x1080,
        /// <summary>
        /// CMSG_REDIRECT_UNK.
        /// </summary>
        ClientConnectionRedirectUnknown1 = 0x1201,
        /// <summary>
        /// CMSG_REDIRECT_AUTH_PROOF.
        /// </summary>
        ClientConnectionRedirectAuthenticationProof = 0x3081,
        /// <summary>
        /// CMSG_REDIRECT_TOKEN_UNKNOWN.
        /// </summary>
        ClientConnectionRedirectUnknown2 = 0x3001,

        // Characters:
        /// <summary>
        /// CMSG_CHAR_CUSTOMIZE.
        /// </summary>
        ClientCharacterCustomize = 0x250,
        /// <summary>
        /// SMSG_CHAR_CUSTOMIZE.
        /// </summary>
        ServerCharacterCustomizeResult = 0xE2B5,
        /// <summary>
        /// CMSG_CHAR_CREATE.
        /// </summary>
        ClientCharacterCreate = 0x2BF0,
        /// <summary>
        /// SMSG_CHAR_CREATE.
        /// </summary>
        ServerCharacterCreateResult = 0xC211,
        /// <summary>
        /// CMSG_CHAR_DELETE.
        /// </summary>
        ClientCharacterDelete = 0x8A78,
        /// <summary>
        /// SMSG_CHAR_DELETE.
        /// </summary>
        ServerCharacterDelete = 0x278,
        /// <summary>
        /// CMSG_CHAR_ENUM.
        /// </summary>
        ClientCharacterEnumeration = 0x3F8,
        /// <summary>
        /// SMSG_CHAR_ENUM.
        /// </summary>
        ServerCharacterEnumerationResult = 0x429C,
        /// <summary>
        /// CMSG_CHAR_RENAME.
        /// </summary>
        ClientCharacterRename = 0xAB38,
        /// <summary>
        /// SMSG_CHAR_RENAME.
        /// </summary>
        ServerCharacterRenameResult = 0xA33C,
        /// <summary>
        /// SMSG_CHARACTER_LOGIN_FAILED.
        /// </summary>
        ServerCharacterLoginFailed = 0xCBD9,

        // Reports:
        /// <summary>
        /// SMSG_COMPLAIN_RESULT.
        /// </summary>
        ServerReportComplaintResult = 0x2295,
        /// <summary>
        /// SMSG_REPORT_PVP_AFK_RESULT.
        /// </summary>
        ServerReportPvpAfkResult = 0x239D,
        /// <summary>
        /// SMSG_GMTICKET_SYSTEMSTATUS.
        /// </summary>
        ServerReportTickSystemStatus = 0x6A51,

        // Chat:
        /// <summary>
        /// SMSG_CHAT_RESTRICTED.
        /// </summary>
        ServerChatRestricted = 0x23BC,
        /// <summary>
        /// SMSG_CHAT_PLAYER_AMBIGUOUS.
        /// </summary>
        ServerChatPlayerNameAmbiguous = 0x2A94,
        /// <summary>
        /// SMSG_CHAT_WRONG_FACTION.
        /// </summary>
        ServerChatWrongFaction = 0xE2D4,

        // Arena teams:
        /// <summary>
        /// SMSG_ARENA_TEAM_CHANGE_FAILED_QUEUED.
        /// </summary>
        ServerArenaTeamChangeFailedQueued = 0xB54,

        // Miscellaneous:
        /// <summary>
        /// SMSG_ADDON_INFO.
        /// </summary>
        ServerAddonInfo = 0xAF9,
        /// <summary>
        /// SMSG_SET_PROFICIENCY.
        /// </summary>
        ServerProficiencySet = 0x22D4,
        /// <summary>
        /// MSG_TALENT_WIPE_CONFIRM.
        /// </summary>
        TalentWipeConfirm = 0x2A95,
        /// <summary>
        /// SMSG_MEETINGSTONE_MEMBER_ADDED.
        /// </summary>
        ServerMeetingStoneMemberAdded = 0x43FD,
        /// <summary>
        /// SMSG_TRIGGER_CINEMATIC.
        /// </summary>
        ServerCinematicTrigger = 0x6310,

        // Cache:
        /// <summary>
        /// SMSG_CLIENTCACHE_VERSION.
        /// </summary>
        ServerCacheVersion = 0xE2B8,
        /// <summary>
        /// SMSG_CREATURE_QUERY_RESPONSE.
        /// </summary>
        ServerQueryCreatureResult = 0x83B8,

        // Realms:
        /// <summary>
        /// CMSG_REALM_SPLIT.
        /// </summary>
        ClientRealmSplit = 0xAB58,
        /// <summary>
        /// SMSG_REALM_SPLIT.
        /// </summary>
        ServerRealmSplitResult = 0x4270,

        // Pets:
        /// <summary>
        /// SMSG_PET_NAME_INVALID.
        /// </summary>
        ServerPetNameInvalid = 0x42D9,
        /// <summary>
        /// SMSG_PETGODMODE.
        /// </summary>
        ServerPetGodMode = 0x8B99,

        // Fishing:
        /// <summary>
        /// SMSG_FISH_NOT_HOOKED.
        /// </summary>
        ServerFishNotHooked = 0xCAB5,
        /// <summary>
        /// SMSG_FISH_ESCAPED.
        /// </summary>
        ServerFishEscaped = 0x431D,

        // Homebinds:
        /// <summary>
        /// SMSG_PLAYERBOUND.
        /// </summary>
        ServerHomebindSuccess = 0x827D,
        /// <summary>
        /// SMSG_BINDPOINTUPDATE.
        /// </summary>
        ServerHomebindUpdate = 0xA255,
        /// <summary>
        /// SMSG_PLAYERBINDERROR.
        /// </summary>
        ServerHomebindError = 0xC3FC,

        // Account data:
        /// <summary>
        /// CMSG_READY_FOR_ACCOUNT_DATA_TIMES.
        /// </summary>
        ClientAccountDataTimes = 0x6A99,
        /// <summary>
        /// SMSG_ACCOUNT_DATA_TIMES.
        /// </summary>
        ServerAccountDataTimesResult = 0x82B5,

        // Experience:
        /// <summary>
        /// SMSG_TOGGLE_XP_GAIN.
        /// </summary>
        ServerExperienceToggleGain = 0x8AB8,
        /// <summary>
        /// SMSG_EXPLORATION_EXPERIENCE.
        /// </summary>
        ServerExperienceExploration = 0x8B58,
        /// <summary>
        /// SMSG_LEVELUP_INFO.
        /// </summary>
        ServerExperienceLevelUpInfo = 0xCB15,

        // Calendar:
        /// <summary>
        /// SMSG_CALENDAR_EVENT_INVITE.
        /// </summary>
        ServerCalendarEventInvite = 0xE2FC,

        // Taxis:
        /// <summary>
        /// SMSG_NEW_TAXI_PATH.
        /// </summary>
        ServerTaxiNewPath = 0xA259,

        // Items:
        /// <summary>
        /// SMSG_ITEM_PUSH_RESULT.
        /// </summary>
        ServerItemPushResult = 0x835D,

        // Loot:
        /// <summary>
        /// MSG_RANDOM_ROLL.
        /// </summary>
        LootRollRandom = 0x8A5D,

        // Objects:
        /// <summary>
        /// SMSG_UPDATE_OBJECT.
        /// </summary>
        ServerObjectUpdate = 0x8BF0,

        // Movement:
        /// <summary>
        /// SMSG_MONSTER_MOVE.
        /// </summary>
        MovementMonsterMove = 0x310,
        /// <summary>
        /// SMSG_MONSTER_MOVE_TRANSPORT.
        /// </summary>
        MovementMonsterMoveTransport = 0x21C
    }
}
