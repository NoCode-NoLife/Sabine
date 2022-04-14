using System;
using System.Threading.Tasks;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Extensions;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;

namespace Sabine.Zone.Network
{
	/// <summary>
	/// Packet handler methods.
	/// </summary>
	public class PacketHandler : PacketHandler<ZoneConnection>
	{
		/// <summary>
		/// Login request sent upon connecting to the server.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ENTER)]
		public void CZ_ENTER(ZoneConnection conn, Packet packet)
		{
			var sessionId = packet.GetInt();
			var characterId = packet.GetInt();
			var accountId = packet.GetInt();
			var sex = packet.GetByte();

			var account = ZoneServer.Instance.Database.GetAccountById(accountId);
			if (account == null)
			{
				conn.Close();
				return;
			}

			if (sessionId != account.SessionId)
			{
				Log.Warning("CZ_ENTER: User '{0}' tried to log in with an invalid session id.", account.Username);
				conn.Close();
				return;
			}

			var character = ZoneServer.Instance.Database.GetCharacter(account, characterId);
			if (character == null)
			{
				Log.Warning("CZ_ENTER: User '{0}' tried to log in with a character that doesn't exist ({1}).", account.Username, characterId);
				conn.Close();
				return;
			}

			if (!ZoneServer.Instance.World.Maps.TryGet(character.MapId, out var map))
			{
				Log.Warning("CZ_ENTER: Map '{0}' not found for character '{1}'.", character.MapId, character.Name);

				var fallbackLocation = new Location(100036, 99, 81); // "prt_vilg02"

				if (!ZoneServer.Instance.World.Maps.TryGet(fallbackLocation.MapId, out map))
				{
					Log.Warning("CZ_ENTER: Fallback map not found either! Abort! Abort!!!");

					conn.Close();
					return;
				}

				character.SetLocation(fallbackLocation);
			}

			conn.Account = account;
			conn.Character = character;
			character.Connection = conn;

			Send.ZC_ACCEPT_ENTER(conn, character);

			map.AddCharacter(character);
		}

		/// <summary>
		/// Notification that the client loaded the map and the character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_NOTIFY_ACTORINIT)]
		public void CZ_NOTIFY_ACTORINIT(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			if (character.IsWarping)
				character.FinalizeWarp();
			else
				character.StartObserving();

			Send.ZC_STATUS(character);
			Send.ZC_PAR_CHANGE(character, ParameterType.Weight, character.Weight / 10);
			Send.ZC_PAR_CHANGE(character, ParameterType.WeightMax, character.WeightMax / 10);
			Send.ZC_PAR_CHANGE(character, ParameterType.SkillPoints, character.SkillPoints);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.BaseExpNeeded, character.BaseExpNeeded);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.JobExpNeeded, character.JobExpNeeded);
		}

		/// <summary>
		/// Request for the current server time.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_TIME)]
		public void CZ_REQUEST_TIME(ZoneConnection conn, Packet packet)
		{
			Send.ZC_NOTIFY_TIME(conn, DateTime.Now);
		}

		/// <summary>
		/// Request to move to a new position.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_MOVE)]
		public void CZ_REQUEST_MOVE(ZoneConnection conn, Packet packet)
		{
			var toPos = (Position)packet.GetPackedPosition();

			var character = conn.GetCurrentCharacter();
			var fromPos = character.Position;
			character.Position = toPos;

			Log.Debug("{0} requests to move from {1},{2} to {3},{4}.", character.Name, fromPos.X, fromPos.Y, toPos.X, toPos.Y);

			Send.ZC_NOTIFY_PLAYERMOVE(character, fromPos, toPos);
			Send.ZC_NOTIFY_MOVE(character, fromPos, toPos);

			var warps = character.Map.GetAllNpcs(a => a.ClassId == 32 && a.Position.InRange(toPos, 2));
			if (warps.Length > 0)
			{
				var warp = warps[0];
				var distance = fromPos.GetDistance(toPos);
				var aproxWalkDur = distance * character.Speed;

				Log.Debug("Warp in {0} ms.", aproxWalkDur);

				Task.Delay(aproxWalkDur).ContinueWith(_ =>
				{
					character.Warp(warp.WarpDestination);
				});
			}

			// Spawn some NPCs to visualize the path the server calculated
			// for this move request.
			if (character.Vars.Temp.GetBool("Sabine.DebugPathEnabled"))
			{
				var path = character.Map.PathFinder.FindPath(fromPos, toPos);
				foreach (var pathPos in path)
				{
					var npc = new Npc(66);
					npc.Warp(character.Map.Id, pathPos);

					Task.Delay(5000).ContinueWith(_ => character.Map.RemoveNpc(npc));
				}
			}
		}

		/// <summary>
		/// Request to say something in chat.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_CHAT)]
		public void CZ_REQUEST_CHAT(ZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort();
			var text = packet.GetString(len - sizeof(short) * 2);

			// The client sends the chat message in the format
			// "name : message", which is inconvenient for us.
			// We'll trim it down to the actual message.
			var index = text.IndexOf(':');
			if (index != -1)
				text = text.Substring(index + 1).Trim();

			var character = conn.GetCurrentCharacter();

			if (ZoneServer.Instance.ChatCommands.TryExecute(character, text))
				return;

			text = string.Format("{0} : {1}", character.Name, text);

			Send.ZC_NOTIFY_CHAT(character, text);
			Send.ZC_NOTIFY_PLAYERCHAT(character, text);
		}

		/// <summary>
		/// Request for a character's name when hovering over them.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQNAME)]
		public void CZ_REQNAME(ZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();

			var character = conn.GetCurrentCharacter();
			var target = character.Map.GetCharacter(handle);

			if (target == null)
			{
				Log.Debug("CZ_REQNAME: User {0} requested the name of a character that doesn't exist.", conn.Account.Username);
				return;
			}

			Send.ZC_ACK_REQNAME(character, target);
		}

		/// <summary>
		/// Request to increase a stat.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_STATUS_CHANGE)]
		public void CZ_STATUS_CHANGE(ZoneConnection conn, Packet packet)
		{
			var type = (ParameterType)packet.GetShort();
			var change = (int)packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var success = false;
			var value = 0;

			if (type < ParameterType.Str || type > ParameterType.Luk)
			{
				Log.Debug("CZ_STATUS_CHANGE: User '{0}' tried to assign points to invalid stat '{1}'.", conn.Account.Username, type);
				goto L_End;
			}

			var pointsNeeded = character.GetStatPointsNeeded(type);
			if (character.StatPoints < pointsNeeded)
			{
				Log.Debug("CZ_STATUS_CHANGE: User '{0}' tried to use more stat points than they have.", conn.Account.Username);
				goto L_End;
			}

			value = character.ModifyStat(type, change);
			character.ModifyStatPoints(-pointsNeeded);

			success = true;

		L_End:
			Send.ZC_STATUS_CHANGE_ACK(character, type, success, value);
		}

		/// <summary>
		/// Request for the amount of players online via the /who command.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_USER_COUNT)]
		public void CZ_REQ_USER_COUNT(ZoneConnection conn, Packet packet)
		{
			var count = ZoneServer.Instance.World.GetPlayerCount();
			Send.ZC_USER_COUNT(conn, count);
		}

		/// <summary>
		/// Request to use an emotion.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_EMOTION)]
		public void CZ_REQ_EMOTION(ZoneConnection conn, Packet packet)
		{
			var emotion = (EmotionId)packet.GetByte();

			if (!Enum.IsDefined(typeof(EmotionId), emotion))
			{
				Log.Warning("CZ_REQ_EMOTION: User '{0}' tried to use the invalid emotion '{1}'.", conn.Account.Username, emotion);
				return;
			}

			var character = conn.GetCurrentCharacter();
			Send.ZC_EMOTION(character, emotion);
		}

		/// <summary>
		/// Request for item description.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ITEM_EXPLANATION_BYNAME)]
		public void CZ_REQ_ITEM_EXPLANATION_BYNAME(ZoneConnection conn, Packet packet)
		{
			var itemName = packet.GetString(16);

			Send.ZC_REQ_ITEM_EXPLANATION_ACK(conn, itemName, "Foobar! Heals 9001 HP.");
		}

		/// <summary>
		/// Request for starting a dialog with an NPC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CONTACTNPC)]
		public void CZ_CONTACTNPC(ZoneConnection conn, Packet packet)
		{
			var targetHandle = packet.GetInt();
			var b1 = packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var target = character.Map.GetCharacter(targetHandle);

			if (target == null)
			{
				Log.Debug("CZ_CONTACTNPC: User '{0}' tried to contact a non-existent target.", conn.Account.Username);
				return;
			}

			if (!(target is Npc npc))
			{
				Log.Debug("CZ_CONTACTNPC: User '{0}' tried to contact a non-NPC.", conn.Account.Username);
				return;
			}

			//Log.Debug("CZ_CONTACTNPC: " + npcHandle);

			//Send.ZC_SAY_DIALOG(character, npcHandle, "Hello, World!");
			//Task.Delay(5000).ContinueWith(_ => Send.ZC_SAY_DIALOG(character, npcHandle, "Goodbye, World!"));
			//Task.Delay(6000).ContinueWith(_ => Send.ZC_WAIT_DIALOG(character, npcHandle));
			//Task.Delay(8000).ContinueWith(_ => Send.ZC_MENU_LIST(character, npcHandle, "Option 1", "Option 2", "End"));

			if (npc.DialogFunc == null)
				return;

			conn.CurrentDialog = new Dialog(character, npc);
			conn.CurrentDialog.Start();
		}

		/// <summary>
		/// Chooses a menu item during a dialog.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHOOSE_MENU)]
		public void CZ_CHOOSE_MENU(ZoneConnection conn, Packet packet)
		{
			var npcHandle = packet.GetInt();
			var choice = packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var npc = character.Map.GetCharacter(npcHandle);

			Log.Debug("CZ_CHOOSE_MENU: {0}, 0x{1:X2}", npcHandle, choice);

			// 0xFF is sent when there's no menu to choose anything from,
			// so it's presumably a cancel action.

			if (conn.CurrentDialog == null)
			{
				Log.Debug("CZ_CHOOSE_MENU: User '{0}' tried to choose a menu item without being in a dialog.", conn.Account.Username);
				return;
			}

			conn.CurrentDialog.Resume(choice.ToString());
		}

		/// <summary>
		/// Request to continue a paused dialog.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_NEXT_SCRIPT)]
		public void CZ_REQ_NEXT_SCRIPT(ZoneConnection conn, Packet packet)
		{
			var npcHandle = packet.GetInt();

			var character = conn.GetCurrentCharacter();
			var npc = character.Map.GetCharacter(npcHandle);

			Log.Debug("CZ_REQ_NEXT_SCRIPT.");

			if (conn.CurrentDialog == null)
			{
				Log.Debug("CZ_CHOOSE_MENU: User '{0}' tried to choose a menu item without being in a dialog.", conn.Account.Username);
				return;
			}

			conn.CurrentDialog.Resume(null);
		}

		/// <summary>
		/// Request to do an action, such as sitting down or attacking.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_ACT)]
		public void CZ_REQUEST_ACT(ZoneConnection conn, Packet packet)
		{
			var targetHandle = packet.GetInt();
			var action = (ActionType)packet.GetByte();

			var character = conn.GetCurrentCharacter();

			switch (action)
			{
				case ActionType.SitDown:
				{
					character.SitDown();
					break;
				}
				case ActionType.StandUp:
				{
					character.StandUp();
					break;
				}
				case ActionType.Attack:
				{
					var target = character.Map.GetCharacter(targetHandle);
					if (target == null)
					{
						Log.Debug("CZ_REQUEST_ACT: User '{0}' tried to attack a character who doesn't exist.", conn.Account.Username);
						return;
					}

					Send.ZC_NOTIFY_ACT(character, character.Handle, target.Handle, DateTime.Now.GetUnixTimestamp(), 10, ActionType.Attack);
					break;
				}
				default:
				{
					Log.Debug("CZ_REQUEST_ACT: Unknown action '{0}'.", action);
					break;
				}
			}
		}
	}
}
