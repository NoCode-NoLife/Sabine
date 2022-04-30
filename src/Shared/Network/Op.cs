﻿namespace Sabine.Shared.Network
{
	/// <summary>
	/// A list of the packet opcodes.
	/// </summary>
	public enum Op
	{
		CA_LOGIN,
		CH_ENTER,
		CH_SELECT_CHAR,
		CH_MAKE_CHAR,
		CH_DELETE_CHAR,
		AC_ACCEPT_LOGIN,
		AC_REFUSE_LOGIN,
		HC_ACCEPT_ENTER,
		HC_REFUSE_ENTER,
		HC_ACCEPT_MAKECHAR,
		HC_REFUSE_MAKECHAR,
		HC_ACCEPT_DELETECHAR,
		HC_REFUSE_DELETECHAR,
		HC_NOTIFY_ZONESVR,
		CZ_ENTER,
		ZC_ACCEPT_ENTER,
		ZC_REFUSE_ENTER,
		ZC_NOTIFY_INITCHAR,
		ZC_NOTIFY_UPDATECHAR,
		ZC_NOTIFY_UPDATEPLAYER,
		ZC_NOTIFY_STANDENTRY,
		ZC_NOTIFY_NEWENTRY,
		ZC_NOTIFY_ACTENTRY,
		ZC_NOTIFY_MOVEENTRY,
		ZC_NOTIFY_STANDENTRY_NPC,
		CZ_NOTIFY_ACTORINIT,
		CZ_REQUEST_TIME,
		ZC_NOTIFY_TIME,
		ZC_NOTIFY_VANISH,
		SC_NOTIFY_BAN,
		CZ_REQUEST_QUIT,
		ZC_ACCEPT_QUIT,
		ZC_REFUSE_QUIT,
		CZ_REQUEST_MOVE,
		ZC_NOTIFY_MOVE,
		ZC_NOTIFY_PLAYERMOVE,
		ZC_STOPMOVE,
		CZ_REQUEST_ACT,
		ZC_NOTIFY_ACT,
		ZC_NOTIFY_ACT_POSITION, // Added in Beta1
		CZ_REQUEST_CHAT,
		ZC_NOTIFY_CHAT,
		ZC_NOTIFY_PLAYERCHAT,
		SERVER_ENTRY_ACK,
		CZ_CONTACTNPC,
		ZC_NPCACK_MAPMOVE,
		ZC_NPCACK_SERVERMOVE,
		ZC_NPCACK_ENABLE,
		CZ_REQNAME,
		ZC_ACK_REQNAME,
		CZ_WHISPER,
		ZC_WHISPER,
		ZC_ACK_WHISPER,
		CZ_BROADCAST,
		ZC_BROADCAST,
		CZ_CHANGE_DIRECTION,
		ZC_CHANGE_DIRECTION,
		ZC_ITEM_ENTRY,
		ZC_ITEM_FALL_ENTRY,
		CZ_ITEM_PICKUP,
		ZC_ITEM_PICKUP_ACK,
		ZC_ITEM_DISAPPEAR,
		CZ_ITEM_THROW,
		ZC_NORMAL_ITEMLIST,
		ZC_EQUIPMENT_ITEMLIST,
		ZC_STORE_NORMAL_ITEMLIST,
		ZC_STORE_EQUIPMENT_ITEMLIST,
		CZ_USE_ITEM,
		ZC_USE_ITEM_ACK,
		CZ_REQ_WEAR_EQUIP,
		ZC_REQ_WEAR_EQUIP_ACK,
		CZ_REQ_TAKEOFF_EQUIP,
		ZC_REQ_TAKEOFF_EQUIP_ACK,
		CZ_REQ_ITEM_EXPLANATION_BYNAME,
		ZC_REQ_ITEM_EXPLANATION_ACK,
		ZC_ITEM_THROW_ACK,
		ZC_PAR_CHANGE,
		ZC_LONGPAR_CHANGE,
		CZ_RESTART,
		ZC_RESTART_ACK,
		ZC_SAY_DIALOG,
		ZC_WAIT_DIALOG,
		ZC_CLOSE_DIALOG,
		ZC_MENU_LIST,
		CZ_CHOOSE_MENU,
		CZ_REQ_NEXT_SCRIPT,
		CZ_REQ_STATUS,
		CZ_STATUS_CHANGE,
		ZC_STATUS_CHANGE_ACK,
		ZC_STATUS,
		ZC_STATUS_CHANGE,
		CZ_REQ_EMOTION,
		ZC_EMOTION,
		CZ_REQ_USER_COUNT,
		ZC_USER_COUNT,
		ZC_SPRITE_CHANGE,
		ZC_SELECT_DEALTYPE,
		CZ_ACK_SELECT_DEALTYPE,
		ZC_PC_PURCHASE_ITEMLIST,
		ZC_PC_SELL_ITEMLIST,
		CZ_PC_PURCHASE_ITEMLIST,
		CZ_PC_SELL_ITEMLIST,
		ZC_PC_PURCHASE_RESULT,
		ZC_PC_SELL_RESULT,
		CZ_DISCONNECT_CHARACTER,
		ZC_ACK_DISCONNECT_CHARACTER,
		CZ_DISCONNECT_ALL_CHARACTER,
		CZ_SETTING_WHISPER_PC,
		CZ_SETTING_WHISPER_STATE,
		ZC_SETTING_WHISPER_PC,
		ZC_SETTING_WHISPER_STATE,
		CZ_REQ_WHISPER_LIST,
		ZC_WHISPER_LIST,
		CZ_CREATE_CHATROOM,
		ZC_ACK_CREATE_CHATROOM,
		ZC_ROOM_NEWENTRY,
		ZC_DESTROY_ROOM,
		CZ_REQ_ENTER_ROOM,
		ZC_REFUSE_ENTER_ROOM,
		ZC_ENTER_ROOM,
		ZC_MEMBER_NEWENTRY,
		ZC_MEMBER_EXIT,
		CZ_CHANGE_CHATROOM,
		ZC_CHANGE_CHATROOM,
		CZ_REQ_ROLE_CHANGE,
		ZC_ROLE_CHANGE,
		CZ_REQ_EXPEL_MEMBER,
		CZ_EXIT_ROOM,
		CZ_REQ_EXCHANGE_ITEM,
		ZC_REQ_EXCHANGE_ITEM,
		CZ_ACK_EXCHANGE_ITEM,
		ZC_ACK_EXCHANGE_ITEM,
		CZ_ADD_EXCHANGE_ITEM,
		ZC_ADD_EXCHANGE_ITEM,
		ZC_ACK_ADD_EXCHANGE_ITEM,
		CZ_CONCLUDE_EXCHANGE_ITEM,
		ZC_CONCLUDE_EXCHANGE_ITEM,
		CZ_CANCEL_EXCHANGE_ITEM,
		ZC_CANCEL_EXCHANGE_ITEM,
		CZ_EXEC_EXCHANGE_ITEM,
		ZC_EXEC_EXCHANGE_ITEM,
		ZC_EXCHANGEITEM_UNDO,
		ZC_NOTIFY_STOREITEM_COUNTINFO,
		CZ_MOVE_ITEM_FROM_BODY_TO_STORE,
		ZC_ADD_ITEM_TO_STORE,
		CZ_MOVE_ITEM_FROM_STORE_TO_BODY,
		ZC_DELETE_ITEM_FROM_STORE,
		CZ_CLOSE_STORE,
		ZC_CLOSE_STORE,
		CZ_MAKE_GROUP,
		ZC_ACK_MAKE_GROUP,
		ZC_GROUP_LIST,
		CZ_REQ_JOIN_GROUP,
		ZC_ACK_REQ_JOIN_GROUP,
		ZC_REQ_JOIN_GROUP,
		CZ_JOIN_GROUP,
		CZ_REQ_LEAVE_GROUP,
		ZC_GROUPINFO_CHANGE,
		CZ_CHANGE_GROUPEXPOPTION,
		CZ_REQ_EXPEL_GROUP_MEMBER,
		ZC_ADD_MEMBER_TO_GROUP,
		ZC_DELETE_MEMBER_FROM_GROUP,
		ZC_NOTIFY_HP_TO_GROUPM,
		ZC_NOTIFY_POSITION_TO_GROUPM,
		CZ_REQUEST_CHAT_PARTY,
		ZC_NOTIFY_CHAT_PARTY,
		ZC_MVP_GETTING_ITEM,
		ZC_MVP_GETTING_SPECIAL_EXP,
		ZC_MVP,
		ZC_THROW_MVPITEM,
		ZC_SKILLINFO_UPDATE,
		ZC_SKILLINFO_LIST,
		ZC_ACK_TOUSESKILL,
		ZC_ADD_SKILL,

		// Added in Beta1
		CZ_UPGRADE_SKILLLEVEL,
		CZ_USE_SKILL,
		ZC_NOTIFY_SKILL,
		ZC_NOTIFY_SKILL_POSITION,
		CZ_USE_SKILL_TOGROUND,
		ZC_NOTIFY_GROUNDSKILL,
		CZ_CANCEL_LOCKON,
		ZC_STATE_CHANGE,
		ZC_USE_SKILL,
		CZ_SELECT_WARPPOINT,
		ZC_WARPLIST,
		CZ_REMEMBER_WARPPOINT,
		ZC_ACK_REMEMBER_WARPPOINT,
		ZC_SKILL_ENTRY,
		ZC_SKILL_DISAPPEAR,
		ZC_NOTIFY_CARTITEM_COUNTINFO,
		ZC_CART_EQUIPMENT_ITEMLIST,
		ZC_CART_NORMAL_ITEMLIST,
		ZC_ADD_ITEM_TO_CART,
		ZC_DELETE_ITEM_FROM_CART,
		CZ_MOVE_ITEM_FROM_BODY_TO_CART,
		CZ_MOVE_ITEM_FROM_CART_TO_BODY,
		CZ_MOVE_ITEM_FROM_STORE_TO_CART,
		CZ_MOVE_ITEM_FROM_CART_TO_STORE,
		CZ_REQ_CARTOFF,
		ZC_CARTOFF,
		ZC_ACK_ADDITEM_TO_CART,
		ZC_OPENSTORE,
		CZ_REQ_CLOSESTORE,
		CZ_REQ_OPENSTORE,
		CZ_REQ_BUY_FROMMC,
		ZC_STORE_ENTRY,
		ZC_DISAPPEAR_ENTRY,
		ZC_PC_PURCHASE_ITEMLIST_FROMMC,
		CZ_PC_PURCHASE_ITEMLIST_FROMMC,
		ZC_PC_PURCHASE_RESULT_FROMMC,
		ZC_PC_PURCHASE_MYITEMLIST,
		ZC_DELETEITEM_FROM_MCSTORE,
		CZ_PKMODE_CHANGE,
		ZC_ATTACK_FAILURE_FOR_DISTANCE,
		ZC_ATTACK_RANGE,
		ZC_ACTION_FAILURE,
		ZC_EQUIP_ARROW,
		ZC_RECOVERY,
		ZC_USESKILL_ACK,
		CZ_ITEM_CREATE,
		CZ_MOVETO_MAP,
		ZC_COUPLESTATUS,
		ZC_OPEN_EDITDLG,
		CZ_INPUT_EDITDLG,
		ZC_COMPASS,
		ZC_SHOW_IMAGE,
		CZ_CLOSE_DIALOG,
		ZC_AUTORUN_SKILL,
		ZC_RESURRECTION,
		CZ_REQ_GIVE_MANNER_POINT,
		ZC_ACK_GIVE_MANNER_POINT,
		ZC_NOTIFY_MANNER_POINT_GIVEN,
		ZC_MYGUILD_BASIC_INFO,
		CZ_REQ_GUILD_MENUINTERFACE,
		ZC_ACK_GUILD_MENUINTERFACE,
		CZ_REQ_GUILD_MENU,
		ZC_GUILD_INFO,
		CZ_REQ_GUILD_EMBLEM_IMG,
		ZC_GUILD_EMBLEM_IMG,
		CZ_REGISTER_GUILD_EMBLEM_IMG,
		ZC_MEMBERMGR_INFO,
		CZ_REQ_CHANGE_MEMBERPOS,
		ZC_ACK_REQ_CHANGE_MEMBERS,
		CZ_REQ_OPEN_MEMBER_INFO,
		ZC_ACK_OPEN_MEMBER_INFO,
		CZ_REQ_LEAVE_GUILD,
		ZC_ACK_LEAVE_GUILD,
		CZ_REQ_BAN_GUILD,
		ZC_ACK_BAN_GUILD,
		CZ_REQ_DISORGANIZE_GUILD,
		ZC_ACK_DISORGANIZE_GUILD_RESULT,
		ZC_ACK_DISORGANIZE_GUILD,
		ZC_POSITION_INFO,
		CZ_REG_CHANGE_GUILD_POSITIONINFO,
		ZC_GUILD_SKILLINFO,
		ZC_BAN_LIST,
		ZC_OTHER_GUILD_LIST,
		CZ_REQ_MAKE_GUILD,
		ZC_POSITION_ID_NAME_INFO,
		ZC_RESULT_MAKE_GUILD,
		CZ_REQ_JOIN_GUILD,
		ZC_ACK_REQ_JOIN_GUILD,
		ZC_REQ_JOIN_GUILD,
		CZ_JOIN_GUILD,
		ZC_UPDATE_GDID,
		ZC_UPDATE_CHARSTAT,
		CZ_GUILD_NOTICE,
		ZC_GUILD_NOTICE,
		CZ_REQ_ALLY_GUILD,
		ZC_REQ_ALLY_GUILD,
		CZ_ALLY_GUILD,
		ZC_ACK_REQ_ALLY_GUILD,
		ZC_ACK_CHANGE_GUILD_POSITIONINFO,
		CZ_REQ_GUILD_MEMBER_INFO,
		ZC_ACK_GUILD_MEMBER_INFO,
		ZC_ITEMIDENTIFY_LIST,
		CZ_REQ_ITEMIDENTIFY,
		ZC_ACK_ITEMIDENTIFY,
		CZ_REQ_ITEMCOMPOSITION_LIST,
		ZC_ITEMCOMPOSITION_LIST,
		CZ_REQ_ITEMCOMPOSITION,
		ZC_ACK_ITEMCOMPOSITION,
		CZ_GUILD_CHAT,
		ZC_GUILD_CHAT,
		CZ_REQ_HOSTILE_GUILD,
		ZC_ACK_REQ_HOSTILE_GUILD,
		ZC_MEMBER_ADD,
		CZ_REQ_DELETE_RELATED_GUILD,
		ZC_DELETE_RELATED_GUILD,
		ZC_ADD_RELATED_GUILD,
		COLLECTORDEAD,
		PING,
		ZC_ACK_ITEMREFINING,
		ZC_NOTIFY_MAPINFO,
		CZ_REQ_DISCONNECT,
		ZC_ACK_REQ_DISCONNECT,
		ZC_MONSTER_INFO,
		ZC_MAKABLEITEMLIST,
		CZ_REQMAKINGITEM,
		ZC_ACK_REQMAKINGITEM,
		CZ_USE_SKILL_TOGROUND_WITHTALKBOX,
		ZC_TALKBOX_CHATCONTENTS,
		ZC_UPDATE_MAPINFO,
		CZ_REQNAME_BYGID,
		ZC_ACK_REQNAME_BYGID,
		ZC_ACK_REQNAMEALL,
		ZC_MSG_STATE_CHANGE,
		CZ_RESET,
		CZ_CHANGE_MAPTYPE,

		// Added in EP4
		ZC_NOTIFY_MAPPROPERTY,
		ZC_NOTIFY_RANKING,
		ZC_NOTIFY_EFFECT,
		CZ_LOCALBROADCAST,
		CZ_CHANGE_EFFECTSTATE,
		ZC_START_CAPTURE,
		CZ_TRYCAPTURE_MONSTER,
		ZC_TRYCAPTURE_MONSTER,
		CZ_COMMAND_PET,
		ZC_PROPERTY_PET,
		ZC_FEED_PET,
		ZC_CHANGESTATE_PET,
		CZ_RENAME_PET,
		ZC_PETEGG_LIST,
		CZ_SELECT_PETEGG,
		CZ_PETEGG_INFO,
		CZ_PET_ACT,
		ZC_PET_ACT,
		ZC_PAR_CHANGE_USER,
		ZC_SKILL_UPDATE,
		ZC_MAKINGARROW_LIST,
		CZ_REQ_MAKINGARROW,
		CZ_REQ_CHANGECART,
		ZC_NPCSPRITE_CHANGE,
		ZC_SHOWDIGIT,
		CZ_REQ_OPENSTORE2,
		ZC_SHOW_IMAGE2,
		ZC_CHANGE_GUILD,
		SC_BILLING_INFO,
		ZC_GUILD_INFO2,
		CZ_GUILD_ZENY,
		ZC_GUILD_ZENY_ACK,
		ZC_DISPEL,
		CZ_REMOVE_AID,
		CZ_SHIFT,
		CZ_RECALL,
		CZ_RECALL_GID,
		AC_ASK_PNGAMEROOM,
		CA_REPLY_PNGAMEROOM,
		CZ_REQ_REMAINTIME,
		ZC_REPLY_REMAINTIME,
		ZC_INFO_REMAINTIME,
		ZC_BROADCAST2,
		ZC_ADD_ITEM_TO_STORE2,
		ZC_ADD_ITEM_TO_CART2,
		CS_REQ_ENCRYPTION,
		SC_ACK_ENCRYPTION,
		ZC_USE_ITEM_ACK2,
		ZC_SKILL_ENTRY2,
		CZ_REQMAKINGHOMUN,
		CZ_MONSTER_TALK,
		ZC_MONSTER_TALK,
		ZC_AUTOSPELLLIST,
		CZ_SELECTAUTOSPELL,
		ZC_DEVOTIONLIST,
		ZC_SPIRITS,
		ZC_BLADESTOP,
		ZC_COMBODELAY,
		ZC_SOUND,
		ZC_OPEN_EDITDLGSTR,
		CZ_INPUT_EDITDLGSTR,
		ZC_NOTIFY_MAPPROPERTY2,
		ZC_SPRITE_CHANGE2,
		ZC_NOTIFY_STANDENTRY2,
		ZC_NOTIFY_NEWENTRY2,
		ZC_NOTIFY_MOVEENTRY2,
		CA_REQ_HASH,
		AC_ACK_HASH,
		CA_LOGIN2,
		ZC_NOTIFY_SKILL2,
		CZ_REQ_ACCOUNTNAME,
		ZC_ACK_ACCOUNTNAME,
		ZC_SPIRITS2,
		ZC_REQ_COUPLE,
		CZ_JOIN_COUPLE,
		ZC_START_COUPLE,
		CZ_REQ_JOIN_COUPLE,
		ZC_COUPLENAME,
		CZ_DORIDORI,
		CZ_MAKE_GROUP2,
		ZC_ADD_MEMBER_TO_GROUP2,
		ZC_CONGRATULATION,
		ZC_NOTIFY_POSITION_TO_GUILDM,
		ZC_GUILD_MEMBER_MAP_CHANGE,
		CZ_CHOPOKGI,
		ZC_NORMAL_ITEMLIST2,
		ZC_CART_NORMAL_ITEMLIST2,
		ZC_STORE_NORMAL_ITEMLIST2,
		AC_NOTIFY_ERROR,
		ZC_UPDATE_CHARSTAT2,
		ZC_NOTIFY_EFFECT2,
		ZC_REQ_EXCHANGE_ITEM2,
		ZC_ACK_EXCHANGE_ITEM2,
		ZC_REQ_BABY,
		CZ_JOIN_BABY,
		ZC_START_BABY,
		CZ_REQ_JOIN_BABY,
		CA_LOGIN3,
		CH_DELETE_CHAR2,
		ZC_REPAIRITEMLIST,
		CZ_REQ_ITEMREPAIR,
		ZC_ACK_ITEMREPAIR,
		ZC_HIGHJUMP,
		CA_CONNECT_INFO_CHANGED,
		ZC_FRIENDS_LIST,
		CZ_ADD_FRIENDS,
		CZ_DELETE_FRIENDS,
		CA_EXE_HASHCHECK,
		ZC_DIVORCE,
		ZC_FRIENDS_STATE,
		ZC_REQ_ADD_FRIENDS,
		CZ_ACK_REQ_ADD_FRIENDS,
		ZC_ADD_FRIENDS_LIST,
		ZC_DELETE_FRIENDS,
		CH_EXE_HASHCHECK,
		CZ_EXE_HASHCHECK,
		HC_BLOCK_CHARACTER,
		ZC_STARSKILL,
	}
}
