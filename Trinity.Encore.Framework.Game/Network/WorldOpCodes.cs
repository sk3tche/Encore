using System;

namespace Trinity.Encore.Framework.Game.Network
{
    /// <summary>
    /// A list of packet opcodes gleaned from client version 4.0.3.13329.
    /// </summary>
    [Serializable]
    public enum WorldOpCodes : uint
    {
        // Authentication:
        /// <summary>
        /// SMSG_AUTH_CHALLENGE.
        /// </summary>
        ServerAuthenticationChallenge = 0x3400,
        /// <summary>
        /// CMSG_AUTH_SESSION.
        /// </summary>
        ClientAuthenticationData = 0x880A,
        /// <summary>
        /// SMSG_AUTH_RESPONSE.
        /// </summary>
        ServerAuthenticationResult = 0x1454,

        // Session:
        /// <summary>
        /// SMSG_LOGOUT_RESPONSE
        /// </summary>
        ServerSessionLogoutResult = 0xE75D,
        // haven't found this in 4.0.3 yet
        /// <summary>
        /// SMSG_LOGOUT_COMPLETE.
        /// </summary>
        ServerSessionLogoutComplete = 0x7756,
        /// <summary>
        /// SMSG_LOGOUT_CANCEL_ACK.
        /// </summary>
        ServerSessionLogoutCancelAck = 0xCD56,
        /// <summary>
        /// SMSG_NEW_WORLD.
        /// </summary>
        ServerSessionNewWorld = 0xE828,
        /// <summary>
        /// SMSG_TIME_SYNC_REQ.
        /// </summary>
        ServerTimeSyncRequest = 0x6F5E,

        // Connection:
        /// <summary>
        /// CMSG_PING.
        /// </summary>
        ClientConnectionPing = 0x882A,
        /// <summary>
        /// SMSG_PONG.
        /// </summary>
        ServerConnectionPong = 0xB000,
        /// <summary>
        /// CMSG_REDIRECT_ERROR.
        /// </summary>
        ServerConnectionRedirectError = 0x0C2A,
        /// <summary>
        /// CMSG_REDIRECT_UNK.
        /// </summary>
        ClientConnectionRedirectUnknown1 = 0x0581,
        /// <summary>
        /// CMSG_REDIRECT_AUTH_PROOF.
        /// </summary>
        ClientConnectionRedirectAuthenticationProof = 0x8C0A,
        // haven't found in 4.0.3 yet
        ///// <summary>
        ///// CMSG_REDIRECT_TOKEN_UNKNOWN.
        ///// </summary>
        //ClientConnectionRedirectUnknown2 = 0x3001,

        // Characters:
        /// <summary>
        /// CMSG_CHAR_CUSTOMIZE.
        /// </summary>
        ClientCharacterCustomize = 0x4F75,
        /// <summary>
        /// SMSG_CHAR_CUSTOMIZE.
        /// </summary>
        ServerCharacterCustomizeResult = 0x5B0A,
        /// <summary>
        /// CMSG_CHAR_CREATE.
        /// </summary>
        ClientCharacterCreate = 0xF47E,
        /// <summary>
        /// SMSG_CHAR_CREATE.
        /// </summary>
        ServerCharacterCreateResult = 0x8C7F,
        /// <summary>
        /// CMSG_CHAR_DELETE.
        /// </summary>
        ClientCharacterDelete = 0xAD5E,
        /// <summary>
        /// SMSG_CHAR_DELETE.
        /// </summary>
        ServerCharacterDelete = 0x7F56,
        /// <summary>
        /// CMSG_CHAR_ENUM.
        /// </summary>
        ClientCharacterEnumeration = 0x6655,
        /// <summary>
        /// SMSG_CHAR_ENUM.
        /// </summary>
        ServerCharacterEnumerationResult = 0x775E,
        /// <summary>
        /// CMSG_CHAR_RENAME.
        /// </summary>
        ClientCharacterRename = 0xCC57,
        /// <summary>
        /// SMSG_CHAR_RENAME.
        /// </summary>
        ServerCharacterRenameResult = 0xA029, // might also be 0xA47D
        /// <summary>
        /// SMSG_CHARACTER_LOGIN_FAILED.
        /// </summary>
        ServerCharacterLoginFailed = 0x2C56,

        // Reports:
        /// <summary>
        /// SMSG_COMPLAIN_RESULT.
        /// </summary>
        ServerReportComplaintResult = 0xAE56,
        /// <summary>
        /// SMSG_REPORT_PVP_AFK_RESULT.
        /// </summary>
        ServerReportPvpAfkResult = 0xA75F,
        /// <summary>
        /// SMSG_GMTICKET_SYSTEMSTATUS.
        /// </summary>
        ServerReportTicketSystemStatus = 0x7C57,

        // Chat:
        /// <summary>
        /// SMSG_CHAT_RESTRICTED.
        /// </summary>
        ServerChatRestricted = 0xF328,
        /// <summary>
        /// SMSG_CHAT_PLAYER_AMBIGUOUS.
        /// </summary>
        ServerChatPlayerNameAmbiguous = 0x3674,
        /// <summary>
        /// SMSG_CHAT_WRONG_FACTION.
        /// </summary>
        ServerChatWrongFaction = 0xC55F,
        /// <summary>
        /// CMSG_CHAT_MSG_SAY.
        /// </summary>
        ClientChatSay = 0x5A90,
        /// <summary>
        /// CMSG_CHAT_MSG_YELL.
        /// </summary>
        ClientChatYell = 0x3A10,
        /// <summary>
        /// SMSG_MESSAGECHAT.
        /// </summary>
        ServerChatMessage = 0x867F,
        /// <summary>
        /// SMSG_CHANNEL_NOTIFY.
        /// </summary>
        ServerNotifyChannel = 0xC120, // maybe 0xC574
        /// <summary>
        /// SMSG_CHANNEL_LIST.
        /// </summary>
        ServerListChannel = 0x7009,
        /// <summary>
        /// SMSG_CHANNEL_MEMBER_COUNT.
        /// </summary>
        ServerChannelMemberCount = 0x0823,
        /// <summary>
        /// SMSG_TEXT_EMOTE.
        /// </summary>
        ServerTextEmote = 0x5B03, // maybe 0x5F57
        /// <summary>
        /// SMSG_ZONE_UNDER_ATTACK.
        /// </summary>
        ServerAlertZoneUnderAttack = 0xE801,
        /// <summary>
        /// SMSG_DEFENSE_MESSAGE.
        /// </summary>
        ServerDefenseMessage = 0x6020,
        /// <summary>
        /// SMSG_SERVER_MESSAGE.
        /// </summary>
        ServerServerMessage = 0x2100,
        /// <summary>
        /// SMSG_RAID_INSTANCE_MESSAGE.
        /// </summary>
        ServerRaidInstanceMessage = 0xD929,
        /// <summary>
        /// SMSG_GM_MESSAGECHAT.
        /// </summary>
        ServerGMChatMessage = 0x2902,

        // Arena teams:
        /// <summary>
        /// SMSG_ARENA_TEAM_CHANGE_FAILED_QUEUED.
        /// </summary>
        ServerArenaTeamChangeFailedQueued = 0xBF5C,

        // Miscellaneous:
        /// <summary>
        /// SMSG_ADDON_INFO.
        /// </summary>
        ServerAddonInfo = 0xEE5D,
        /// <summary>
        /// SMSG_SET_PROFICIENCY.
        /// </summary>
        ServerProficiencySet = 0xF101, // maybe 0xF555
        /// <summary>
        /// MSG_TALENT_WIPE_CONFIRM.
        /// </summary>
        TalentWipeConfirm = 0xEB28,
        /// <summary>
        /// SMSG_MEETINGSTONE_MEMBER_ADDED.
        /// </summary>
        ServerMeetingStoneMemberAdded = 0x4228,
        /// <summary>
        /// SMSG_TRIGGER_CINEMATIC.
        /// </summary>
        ServerCinematicTrigger = 0x129,
        /// <summary>
        /// SMSG_PLAY_SOUND.
        /// </summary>
        ServerPlaySound = 0xA47F,
        /// <summary>
        /// SMSG_ACTION_BUTTONS.
        /// </summary>
        ServerActionButtons = 0x4120, // maybe 0x4574
        /// <summary>
        /// CMSG_PLAYED_TIME.
        /// </summary>
        ClientPlayedTime = 0x7E5E,
        /// <summary>
        /// SMSG_PLAYED_TIME.
        /// </summary>
        ServerPlayedTime = 0x4108,
        /// <summary>
        /// SMSG_MOTD
        /// </summary>
        ServerMessageOfTheDay = 0x0328, // maybe 0x077C
        /// <summary>
        /// SMSG_NOTIFICATION.
        /// </summary>
        ServerNotification = 0x620A,

        // Cache:
        /// <summary>
        /// SMSG_CLIENTCACHE_VERSION.
        /// </summary>
        ServerCacheVersion = 0xCE74,
        /// <summary>
        /// CMSG_CREATURE_QUERY.
        /// </summary>
        ClientQueryCreature = 0x8454,
        /// <summary>
        /// SMSG_CREATURE_QUERY_RESPONSE.
        /// </summary>
        ServerQueryCreatureResult = 0xE45E,
        /// <summary>
        /// CMSG_NAME_QUERY.
        /// </summary>
        ClientQueryName = 0xC57E,
        /// <summary>
        /// SMSG_NAME_QUERY_RESPONSE.
        /// </summary>
        ServerQueryNameResponse = 0x4D5E,

        // Realms:
        /// <summary>
        /// CMSG_REALM_SPLIT.
        /// </summary>
        ClientRealmSplit = 0x477D,
        /// <summary>
        /// SMSG_REALM_SPLIT.
        /// </summary>
        ServerRealmSplitResult = 0x3454, // unsure

        // Pets:
        /// <summary>
        /// SMSG_PET_NAME_INVALID.
        /// </summary>
        ServerPetNameInvalid = 0x1003, // maybe 0x1457
        /// <summary>
        /// SMSG_PETGODMODE.
        /// </summary>
        ServerPetGodMode = 0x2C54,

        // Fishing:
        /// <summary>
        /// SMSG_FISH_NOT_HOOKED.
        /// </summary>
        ServerFishNotHooked = 0x3F76,
        /// <summary>
        /// SMSG_FISH_ESCAPED.
        /// </summary>
        ServerFishEscaped = 0x1F77,

        // Homebinds:
        /// <summary>
        /// SMSG_PLAYERBOUND.
        /// </summary>
        ServerHomebindSuccess = 0x5B23, // maybe 0x5F77
        /// <summary>
        /// SMSG_BINDPOINTUPDATE.
        /// </summary>
        ServerHomebindUpdate = 0x175D,
        /// <summary>
        /// SMSG_PLAYERBINDERROR.
        /// </summary>
        ServerHomebindError = 0x765C,

        // Account data:
        /// <summary>
        /// CMSG_READY_FOR_ACCOUNT_DATA_TIMES.
        /// </summary>
        ClientAccountDataTimes = 0xD677,
        /// <summary>
        /// SMSG_ACCOUNT_DATA_TIMES.
        /// </summary>
        ServerAccountDataTimesResult = 0xFD55,
        /// <summary>
        /// CMSG_UPDATE_ACCOUNT_DATA.
        /// </summary>
        ClientUpdateAccountData = 0xFF7E,

        // Experience:
        /// <summary>
        /// SMSG_TOGGLE_XP_GAIN.
        /// </summary>
        ServerExperienceToggleGain = 0xBC55,
        /// <summary>
        /// SMSG_EXPLORATION_EXPERIENCE.
        /// </summary>
        ServerExperienceExploration = 0xA022, // maybe 0xA476
        /// <summary>
        /// SMSG_LEVELUP_INFO.
        /// </summary>
        ServerExperienceLevelUpInfo = 0x092A,
        /// <summary>
        /// SMSG_LOG_XPGAIN.
        /// </summary>
        ServerXPGained = 0x7202,

        // Calendar:
        /// <summary>
        /// SMSG_CALENDAR_EVENT_INVITE.
        /// </summary>
        ServerCalendarEventInvite = 0xF803, // maybe 0xFC57

        // Taxis:
        /// <summary>
        /// SMSG_NEW_TAXI_PATH.
        /// </summary>
        ServerTaxiNewPath = 0xAE5E,

        // Items:
        /// <summary>
        /// SMSG_ITEM_PUSH_RESULT.
        /// </summary>
        ServerItemPushResult = 0xDB00,

        // Loot:
        /// <summary>
        /// MSG_RANDOM_ROLL.
        /// </summary>
        LootRollRandom = 0xE001, // maybe 0xE455

        // Objects:
        /// <summary>
        /// SMSG_UPDATE_OBJECT.
        /// </summary>
        ServerObjectUpdate = 0xFC7D,
        /// <summary>
        /// SMSG_DESTROY_OBJECT.
        /// </summary>
        ServerObjectDestroy = 0x6F77,
        /// <summary>
        /// SMSG_COMPRESSED_UPDATE_OBJECT.
        /// </summary>
        ServerCompressedObjectUpdate = 0x6C7D,

        // Movement:
        /// <summary>
        /// SMSG_MONSTER_MOVE.
        /// </summary>
        MovementMonsterMove = 0xA65D,
        /// <summary>
        /// SMSG_MONSTER_MOVE_TRANSPORT.
        /// </summary>
        MovementMonsterMoveTransport = 0x777C,
        /// <summary>
        /// MSG_MOVE_START_FORWARD.
        /// </summary>
        MovementStartForward = 0xF576,
        /// <summary>
        /// MSG_MOVE_START_BACKWARD
        /// </summary>
        MovementStartBackwards = 0xCC7C,
        /// <summary>
        /// MSG_MOVE_STOP.
        /// </summary>
        MovementStop = 0x4E76,
        /// <summary>
        /// MSG_MOVE_START_STRAFE_LEFT.
        /// </summary>
        MovementStartLeftStrafe = 0x5F5C,
        /// <summary>
        /// MSG_MOVE_START_STRAFE_RIGHT.
        /// </summary>
        MovementStartRightStrafe = 0x265C,
        /// <summary>
        /// MSG_MOVE_STOP_STRAFE.
        /// </summary>
        MovementStopStrafe = 0xD7F,
        /// <summary>
        /// MSG_MOVE_JUMP.
        /// </summary>
        MovementJump = 0x7477,
        /// <summary>
        /// MSG_MOVE_START_TURN_LEFT.
        /// </summary>
        MovementStartLeftTurn = 0x945F,
        /// <summary>
        /// MSG_MOVE_START_TURN_RIGHT.
        /// </summary>
        MovementStartRightTurn = 0x6657,
        /// <summary>
        /// MSG_MOVE_STOP_TURN.
        /// </summary>
        MovementStopTurning = 0x6D54,
        /// <summary>
        /// MSG_MOVE_SET_RUN_MODE.
        /// </summary>
        MovementRunMode = 0x7D56,
        /// <summary>
        /// MSG_MOVE_SET_WALK_MODE.
        /// </summary>
        MovementWalkMode = 0xF75D,
        /// <summary>
        /// MSG_MOVE_FALL_LAND.
        /// </summary>
        MovementFallLand = 0xF474,
        /// <summary>
        /// MSG_MOVE_HEARTBEAT.
        /// </summary>
        MovementHeartbeat = 0x177C,
        /// <summary>
        /// MSG_MOVE_START_ASCEND.
        /// </summary>
        MovementStartAscent = 0x2656,
        /// <summary>
        /// MSG_MOVE_START_DESCEND.
        /// </summary>
        MovementStartDescent = 0x877D,
        /// <summary>
        /// MSG_MOVE_STOP_ASCEND.
        /// </summary>
        MovementStopAscent = 0x5D54,
        /// <summary>
        /// MSG_MOVE_START_PITCH_UP.
        /// </summary>
        MovementStartPitchUp = 0x1677,
        /// <summary>
        /// MSG_MOVE_START_PITCH_DOWN.
        /// </summary>
        MovementStartPitchDown = 0xCE75,
        /// <summary>
        /// MSG_MOVE_STOP_PITCH.
        /// </summary>
        MovementStopPitch = 0x4E7D,
        /// <summary>
        /// MSG_MOVE_TELEPORT.
        /// </summary>
        MovementTeleport = 0xC557,
        /// <summary>
        /// MSG_MOVE_SET_FACING.
        /// </summary>
        MovementSetFacing = 0x865D,
        /// <summary>
        /// MSG_MOVE_SET_PITCH.
        /// </summary>
        MovementSetPitch = 0x0E7C,
        // We have:
        // MSG_MOVE_TOGGLE_COLLISION_CHEAT (maybe): 0xE903
        // MSG_MOVE_UNKNOWN_1234: 0x1E7F # aka 7807
        // MSG_MOVE_UNK: 0x5D5C # aka 23900
        /// <summary>
        /// MSG_MOVE_ROOT.
        /// </summary>
        MovementRoot = 0x9555,
        /// <summary>
        /// MSG_MOVE_UNROOT.
        /// </summary>
        MovementUnroot = 0xFC55,
        /// <summary>
        /// MSG_MOVE_START_SWIM.
        /// </summary>
        MovementStartSwim = 0xAE57,
        /// <summary>
        /// MSG_MOVE_STOP_SWIM.
        /// </summary>
        MovementStopSwim = 0xAC7D,
        /// <summary>
        /// MSG_MOVE_START_SWIM_CHEAT.
        /// </summary>
        MovementStartSwimCheat = 0x2755,
        /// <summary>
        /// MSG_MOVE_STOP_SWIM_CHEAT.
        /// </summary>
        MovementStopSwimCheat = 0x3D54,

        // Social:
        /// <summary>
        /// CMSG_CONTACT_LIST.
        /// </summary>
        ClientContactList = 0xCD5D,
        /// <summary>
        /// SMSG_CONTACT_LIST.
        /// </summary>
        ServerContactListResponse = 0x1221, // maybe 0x1675
        /// <summary>
        /// SMSG_FRIEND_STATUS.
        /// </summary>
        ServerFriendStatus = 0xBB22,
        /// <summary>
        /// CMSG_ADD_FRIEND.
        /// </summary>
        ClientAddFriend = 0x6E5F,

        // Spells:
        /// <summary>
        /// SMSG_INITIAL_SPELLS.
        /// </summary>
        ServerInitialSpells = 0x5209, // maybe 0x565D
        /// <summary>
        /// SMSG_LEARNED_SPELL.
        /// </summary>
        ServerLearnedSpell = 0x9004,

        // LFG:
        /// <summary>
        /// SMSG_LFG_BOOT_PLAYER.
        /// </summary>
        ServerLFGBootPlayer = 0xC802, // maybe 0xCC56

        // Instances:
        /// <summary>
        /// SMSG_INSTANCE_RESET.
        /// </summary>
        ServerResetInstance = 0x1B28,
        /// <summary>
        /// SMSG_INSTANCE_RESET_FAILED.
        /// </summary>
        ServerInstanceResetFailed = 0xD208,
        /// <summary>
        /// SMSG_UPDATE_LAST_INSTANCE.
        /// </summary>
        ServerUpdateLastInstance = 0x9B21,
        /// <summary>
        /// SMSG_UPDATE_INSTANCE_OWNERSHIP.
        /// </summary>
        ServerUpdateInstanceOwnership = 0x8321,
        /// <summary>
        /// SMSG_RESET_FAILED_NOTIFY.
        /// </summary>
        ServerResetFailedNotification = 0xB908,

        // Titles:
        /// <summary>
        /// SMSG_TITLE_EARNED.
        /// </summary>
        ServerEarnedTitle = 0x420B,

        // Achievements:
        /// <summary>
        /// SMSG_SERVER_FIRST_ACHIEVEMENT.
        /// </summary>
        ServerAlertRealmFirstAchievement = 0xA92A,
    }
}
