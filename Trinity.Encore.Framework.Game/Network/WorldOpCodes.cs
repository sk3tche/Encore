using System;

namespace Trinity.Encore.Framework.Game.Network
{
    /// <summary>
    /// A list of packet opcodes gleaned from client version 4.0.3.13329.
    /// </summary>
    [Serializable]
    public enum WorldOpCodes : uint
    {
        #region Authentication
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
        /// <summary>
        /// SMSG_SEND_QUEUED_PACKETS
        /// </summary>
        ServerSendQueuedPackets = 0x1400,
        #endregion

        #region Session
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
        #endregion

        #region Connection
        /// <summary>
        /// CMSG_PING.
        /// </summary>
        ClientConnectionPing = 0x882A,
        /// <summary>
        /// SMSG_PONG.
        /// </summary>
        ServerConnectionPong = 0xB000,
        /// <summary>
        /// SMSG_REDIRECT_CLIENT.
        /// </summary>
        ServerRedirectClient = 0x9000,
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
        /// <summary>
        /// SMSG_TRANSFER_PENDING.
        /// </summary>
        ServerTransferPending = 0x502B,
        /// <summary>
        /// SMSG_TRANSFER_ABORTED.
        /// </summary>
        ServerTransferAborted = 0x0A2A,
        /// <summary>
        /// SMSG_KICK_REASON.
        /// </summary>
        ServerKickReason = 0x3320,
        /// <summary>
        /// CMSG_TIME_SYNC_RESP.
        /// </summary>
        ClientTimeSyncResponse = 0x0D57,
        /// <summary>
        /// CMSG_LOGOUT_REQUEST.
        /// </summary>
        ClientLogoutRequest = 0x8E56,
        #endregion

        #region Characters
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
        /// <summary>
        /// SMSG_FACTION_CHANGE.
        /// </summary>
        ServerCharacterFactionChange = 0xC822,
        /// <summary>
        /// CMSG_PLAYER_LOGIN.
        /// </summary>
        ClientPlayerLogin = 0x05A1,
        #endregion

        #region Reports
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
        #endregion

        #region Chat
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
        /// CMSG_CHAT_MSG_CHANNEL_SAY.
        /// </summary>
        ClientChannelSay = 0x3A88,
        /// <summary>
        /// CMSG_CHAT_MSG_GUILD.
        /// </summary>
        ClientGuildSay = 0x2A88,
        /// <summary>
        /// CMSG_CHAT_MSG_WHISPER.
        /// </summary>
        ClientWhisper = 0x5A80,
        /// <summary>
        /// CMSG_CHAT_MSG_AFK.
        /// </summary>
        ClientAFKMessage = 0x6A88,
        /// <summary>
        /// CMSG_CHAT_MSG_DND.
        /// </summary>
        ClientDNDMessage = 0x3A00,
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
        /// CMSG_JOIN_CHANNEL.
        /// </summary>
        ClientJoinChannel = 0x3A98,
        /// <summary>
        /// CMSG_EMOTE.
        /// </summary>
        ClientEmote = 0x7F5C,
        /// <summary>
        /// CMSG_TEXT_EMOTE.
        /// </summary>
        ClientTextEmote= 0x4A90,
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
        #endregion

        #region Arena Teams
        /// <summary>
        /// SMSG_ARENA_TEAM_CHANGE_FAILED_QUEUED.
        /// </summary>
        ServerArenaTeamChangeFailedQueued = 0xBF5C,
        /// <summary>
        /// SMSG_ARENA_TEAM_ROSTER.
        /// </summary>
        ServerArenaTeamRoster = 0xA80A,
        /// <summary>
        /// SMSG_ARENA_TEAM_STATS.
        /// </summary>
        ServerArenaTeamStats = 0x9B0B,
        /// <summary>
        /// MSG_INSPECT_ARENA_TEAMS.
        /// </summary>
        InspectArenaTeams = 0x6108,
        /// <summary>
        /// SMSG_ARENA_OPPONENT_UPDATE.
        /// </summary>
        ServerArenaOpponentUpdate = 0x5B29,
        #endregion

        #region Miscellaneous
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
        /// CMSG_SET_ACTION_BUTTON.
        /// </summary>
        ClientSetActionButton = 0x355C,
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
        /// <summary>
        /// SMSG_INVALIDATE_PLAYER.
        /// </summary>
        ServerInvalidatePlayer = 0xFB0A,
        /// <summary>
        /// SMSG_EXPECTED_SPAM_RECORDS.
        /// </summary>
        ServerExpectedSpamRecords = 0xA108,
        /// <summary>
        /// SMSG_DURABILITY_DAMAGE_DEATH.
        /// </summary>
        ServerDurabilityDamageDeath = 0xE328,
        /// <summary>
        /// CMSG_SET_SELECTION.
        /// </summary>
        ClientSetSelection = 0x5577,
        /// <summary>
        /// CMSG_ZONEUPDATE.
        /// </summary>
        ClientZoneUpdate = 0x5C7D,
        #endregion

        #region Cache
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
        /// <summary>
        /// CMSG_NPC_TEXT_QUERY.
        /// </summary>
        ClientQueryNPCText = 0x5654,
        /// <summary>
        /// SMSG_NPC_TEXT_RESPONSE.
        /// </summary>
        ServerNPCTextResponse = 0x320A,
        /// <summary>
        /// SMSG_QUEST_QUERY_RESPONSE.
        /// </summary>
        ServerQueryQuestResponse = 0x720B,
        /// <summary>
        /// CMSG_PAGE_TEXT_QUERY.
        /// </summary>
        ClientQueryPageText = 0x2C75,
        /// <summary>
        /// SMSG_PAGE_TEXT_QUERY_RESPONSE.
        /// </summary>
        ServerQueryPageTextResponse = 0x730B,
        /// <summary>
        /// SMSG_PET_NAME_QUERY_RESPONSE.
        /// </summary>
        ServerQueryPetNameResponse = 0xE20A,
        /// <summary>
        /// SMSG_PETITION_QUERY_RESPONSE.
        /// </summary>
        ServerQueryPetitionResponse = 0x7301,
        /// <summary>
        /// SMSG_ITEM_TEXT_QUERY_RESPONSE.
        /// </summary>
        ServerQueryItemTextResponse = 0xA929,
        /// <summary>
        /// SMSG_ARENA_TEAM_QUERY_RESPONSE.
        /// </summary>
        ServerQueryArenaTeamResponse = 0xC02B,
        /// <summary>
        /// SMSG_DANCE_QUERY_RESPONSE.
        /// </summary>
        ServerQueryDanceResponse = 0x7800,
        /// <summary>
        /// SMSG_GUILD_QUERY_RESPONSE.
        /// </summary>
        ServerQueryGuildResponse = 0x365C,
        /// <summary>
        /// CMSG_GAMEOBJECT_QUERY.
        /// </summary>
        ClientGameobjectQuery = 0x0455,
        /// <summary>
        /// SMSG_GAMEOBJECT_QUERY_RESPONSE.
        /// </summary>
        ServerGameobjectQueryResponse = 0x0577,
        #endregion

        #region Realms
        /// <summary>
        /// CMSG_REALM_SPLIT.
        /// </summary>
        ClientRealmSplit = 0x477D,
        /// <summary>
        /// SMSG_REALM_SPLIT.
        /// </summary>
        ServerRealmSplitResult = 0x3454, // unsure
        #endregion

        #region Pets
        /// <summary>
        /// SMSG_PET_NAME_INVALID.
        /// </summary>
        ServerPetNameInvalid = 0x1003, // maybe 0x1457
        /// <summary>
        /// SMSG_PETGODMODE.
        /// </summary>
        ServerPetGodMode = 0x2C54,
        /// <summary>
        /// SMSG_PET_SPELLS.
        /// </summary>
        ServerPetSpells = 0x5928,
        /// <summary>
        /// SMSG_PET_MODE.
        /// </summary>
        ServerPetMode = 0xFA0B,
        /// <summary>
        /// SMSG_PET_ACTION_FEEDBACK.
        /// </summary>
        ServerPetActionFeedback = 0xA800,
        /// <summary>
        /// SMSG_PET_BROKEN.
        /// </summary>
        ServerPetBroken = 0xE92B,
        /// <summary>
        /// SMSG_PET_RENAMEABLE.
        /// </summary>
        ServerPetRenameable = 0x520A,
        /// <summary>
        /// SMSG_PET_UPDATE_COMBO_POINTS.
        /// </summary>
        ServerPetUpdateComboPoints = 0xD20B,
        /// <summary>
        /// SMSG_PET_GUIDS.
        /// </summary>
        ServerPetGUIDs = 0xFA08,
        /// <summary>
        /// MSG_LIST_STABLED_PETS.
        /// </summary>
        ListStabledPets = 0x5A09,
        /// <summary>
        /// SMSG_STABLE_RESULT.
        /// </summary>
        ServerStableResult = 0xE300,
        #endregion

        #region Fishing
        /// <summary>
        /// SMSG_FISH_NOT_HOOKED.
        /// </summary>
        ServerFishNotHooked = 0x3F76,
        /// <summary>
        /// SMSG_FISH_ESCAPED.
        /// </summary>
        ServerFishEscaped = 0x1F77,
        #endregion

        #region Homebinds
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
        #endregion

        #region Account Data
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
        #endregion

        #region Experience
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
        #endregion

        #region Calendar
        /// <summary>
        /// SMSG_CALENDAR_EVENT_INVITE.
        /// </summary>
        ServerCalendarEventInvite = 0xF803, // maybe 0xFC57
        #endregion

        #region Taxis
        /// <summary>
        /// SMSG_NEW_TAXI_PATH.
        /// </summary>
        ServerTaxiNewPath = 0xAE5E,
        #endregion

        #region Items
        /// <summary>
        /// SMSG_ITEM_PUSH_RESULT.
        /// </summary>
        ServerItemPushResult = 0xDB00,
        #endregion

        #region Loot
        /// <summary>
        /// MSG_RANDOM_ROLL.
        /// </summary>
        LootRollRandom = 0xE001, // maybe 0xE455
        /// <summary>
        /// CMSG_LOOT.
        /// </summary>
        ClientLoot = 0xBD77,
        #endregion

        #region Objects
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
        #endregion

        #region Movement
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
        #endregion

        #region Social
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
        /// <summary>
        /// SMSG_WHO.
        /// </summary>
        ServerWho = 0xCB28,
        /// <summary>
        /// SMSG_WHOIS.
        /// </summary>
        ServerWhois = 0x3328,
        /// <summary>
        /// SMSG_RWHOIS.
        /// </summary>
        ServerRWhois = 0x3228,
        #endregion

        #region Spells
        /// <summary>
        /// SMSG_INITIAL_SPELLS.
        /// </summary>
        ServerInitialSpells = 0x5209, // maybe 0x565D
        /// <summary>
        /// SMSG_LEARNED_SPELL.
        /// </summary>
        ServerLearnedSpell = 0x9004,
        /// <summary>
        /// CMSG_CAST_SPELL.
        /// </summary>
        ClientCastSpell = 0x4C56,
        /// <summary>
        /// CMSG_CANCEL_CHANNELLING.
        /// </summary>
        ClientCancelChannelling = 0x957C,
        #endregion

        #region Auras
        /// <summary>
        /// CMSG_CANCEL_AURA.
        /// </summary>
        ClientCancelAura = 0x545E,
        #endregion

        #region LFG
        /// <summary>
        /// SMSG_LFG_BOOT_PLAYER.
        /// </summary>
        ServerLFGBootPlayer = 0xC802, // maybe 0xCC56
        #endregion

        #region Instances
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
        #endregion

        #region Titles
        /// <summary>
        /// SMSG_TITLE_EARNED.
        /// </summary>
        ServerEarnedTitle = 0x420B,
        #endregion

        #region Achievements
        /// <summary>
        /// SMSG_SERVER_FIRST_ACHIEVEMENT.
        /// </summary>
        ServerAlertRealmFirstAchievement = 0xA92A,
        #endregion

        #region Dances
        /// <summary>
        /// SMSG_INVALIDATE_DANCE.
        /// </summary>
        ServerInvalidateDance = 0x9229,
        /// <summary>
        /// SMSG_PLAY_DANCE.
        /// </summary>
        ServerPlayDance = 0xC203,
        /// <summary>
        /// SMSG_STOP_DANCE.
        /// </summary>
        ServerStopDance = 0x4002,
        /// <summary>
        /// SMSG_NOTIFY_DANCE.
        /// </summary>
        ServerNotifyDance = 0xE308,
        /// <summary>
        /// SMSG_LEARNED_DANCE_MOVES.
        /// </summary>
        ServerLearnedDanceMoves = 0xF209,
        #endregion

        #region Warden
        /// <summary>
        /// SMSG_WARDEN_DATA.
        /// </summary>
        ServerWardenData = 0x212B,
        #endregion

        #region Tutorials
        /// <summary>
        /// SMSG_TUTORIAL_FLAGS.
        /// </summary>
        ServerTutorialFlags = 0x8203, // maybe 0x8657?
        #endregion

        #region Gossip
        /// <summary>
        /// SMSG_GOSSIP_MESSAGE.
        /// </summary>
        ServerGossipMessage = 0xD12A,
        /// <summary>
        /// SMSG_GOSSIP_COMPLETE.
        /// </summary>
        ServerGossipComplete = 0x430B,
        /// <summary>
        /// SMSG_GOSSIP_POI.
        /// </summary>
        ServerGossipPointOfInterest = 0x1002,
        #endregion

        #region Duels
        /// <summary>
        /// SMSG_DUEL_REQUESTED
        /// </summary>
        ServerDuelRequested = 0x530A,
        /// <summary>
        /// SMSG_DUEL_OUTOFBOUNDS.
        /// </summary>
        ServerDuelOutOfBounds = 0xB829,
        /// <summary>
        /// SMSG_DUEL_INBOUNDS.
        /// </summary>
        ServerDuelInbounds = 0x9B00,
        /// <summary>
        /// SMSG_DUEL_COUNTDOWN.
        /// </summary>
        ServerDuelCountdown = 0x8A21,
        /// <summary>
        /// SMSG_DUEL_COMPLETE.
        /// </summary>
        ServerDuelComplete = 0xE323,
        /// <summary>
        /// SMSG_DUEL_WINNER.
        /// </summary>
        ServerDuelWinner = 0x2329,
        #endregion

        #region Minigame
        /// <summary>
        /// SMSG_MINIGAME_SETUP.
        /// </summary>
        ServerMinigameSetup = 0x1824,
        /// <summary>
        /// SMSG_MINIGAME_STATE.
        /// </summary>
        ServerMinigameState = 0x482B,
        #endregion

        #region Guild Bank
        /// <summary>
        /// SMSG_GUILD_BANK_LIST.
        /// </summary>
        ServerGuildBankList = 0xB822,
        /// <summary>
        /// MSG_GUILD_BANK_LOG_QUERY.
        /// </summary>
        GuildBankLogQuery = 0x6308,
        /// <summary>
        /// MSG_GUILD_BANK_MONEY_WITHDRAWN.
        /// </summary>
        GuildBankMoneyWithdraw = 0x302A,
        /// <summary>
        /// MSG_QUERY_GUILD_BANK_TEXT.
        /// </summary>
        QueryGuildBankText = 0x2322,
        #endregion

        #region Auctions
        /// <summary>
        /// MSG_AUCTION_HELLO.
        /// </summary>
        ActionHello = 0xD120,
        /// <summary>
        /// SMSG_AUCTION_COMMAND_RESULT.
        /// </summary>
        ServerAuctionCommandResult = 0xEB22,
        /// <summary>
        /// SMSG_AUCTION_BIDDER_LIST_RESULT.
        /// </summary>
        ServerAuctionBidderListResult = 0xEA0A,
        /// <summary>
        /// SMSG_AUCTION_OWNER_LIST_RESULT.
        /// </summary>
        ServerAuctionOwnerListResult = 0xDA22,
        /// <summary>
        /// SMSG_AUCTION_LIST_RESULT.
        /// </summary>
        ServerAuctionListResult = 0xAB03,
        /// <summary>
        /// SMSG_AUCTION_BIDDER_NOTIFICATION.
        /// </summary>
        ServerActionBidderNotification = 0x3021,
        /// <summary>
        /// SMSG_AUCTION_OWNER_NOTIFICATION.
        /// </summary>
        ServerAuctionOwnerNotification = 0xC009,
        /// <summary>
        /// SMSG_AUCTION_REMOVED_NOTIFICATION.
        /// </summary>
        ServerAuctionRemovedNotification = 0x0A0A,
        /// <summary>
        /// SMSG_AUCTION_LIST_PENDING_SALES.
        /// </summary>
        ServerAuctionListPendingSales = 0xDB29,
        #endregion

        #region Mail
        /// <summary>
        /// SMSG_SEND_MAIL_RESULT.
        /// </summary>
        ServerSendMailResult = 0x8B23,
        /// <summary>
        /// SMSG_MAIL_LIST_RESULT.
        /// </summary>
        ServerMailListResult = 0x3804,
        /// <summary>
        /// MSG_QUERY_NEXT_MAIL_TIME.
        /// </summary>
        QueryNextMailTime = 0x7003,
        /// <summary>
        /// SMSG_RECEIVED_MAIL.
        /// </summary>
        ServerReceivedMail = 0x2122,
        #endregion

        #region Groups and Raid Groups
        /// <summary>
        /// MSG_RAID_TARGET_UPDATE.
        /// </summary>
        RaidTargetUpdate = 0x380B,
        /// <summary>
        /// MSG_RAID_READY_CHECK.
        /// </summary>
        RaidReadyCheck = 0x8B22,
        /// <summary>
        /// MSG_RAID_READY_CHECK_CONFIRM.
        /// </summary>
        RaidReadyCheckConfirm = 0xB123,
        /// <summary>
        /// SMSG_RAID_READY_CHECK_FINISHED.
        /// </summary>
        ServerRaidReadyCheckFinished = 0xF821,
        /// <summary>
        /// SMSG_RAID_READY_CHECK_ERROR.
        /// </summary>
        ServerRaidReadyCheckError = 0x502A,
        /// <summary>
        /// MSG_NOTIFY_PARTY_SQUELCH.
        /// </summary>
        NotifyPartySquelch = 0xE120,
        /// <summary>
        /// SMSG_ECHO_PARTY_SQUELCH.
        /// </summary>
        ServerEchoPartySquelch = 0x1303,
        #endregion

        #region Guilds
        /// <summary>
        /// MSG_GUILD_PERMISSIONS.
        /// </summary>
        GuildPermissions = 0x2A00,
        /// <summary>
        /// MSG_GUILD_EVENT_LOG_QUERY.
        /// </summary>
        GuildEventLogQuery = 0xF90A,
        #endregion

        #region Battlegrounds
        #endregion

        #region Battlefields
        /// <summary>
        /// SMSG_BATTLEFIELD_LIST.
        /// </summary>
        ServerBattlefieldList = 0x3858,
        #endregion

        #region Trade
        /// <summary>
        /// CMSG_CANCEL_TRADE.
        /// </summary>
        ClientCancelTrade = 0x0C2A,
        #endregion
    }
}
