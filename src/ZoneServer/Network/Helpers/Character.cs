using System;
using Sabine.Shared;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Network.Helpers
{
	/// <summary>
	/// Character-related extension methods for Packet.
	/// </summary>
	public static class PacketCharacterHelpers
	{
		/// <summary>
		/// Adds stand entry information about character to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="character"></param>
		public static void AddStandEntry(this Packet packet, IEntryCharacter character)
		{
			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);

			if (Game.Version < Versions.EP3_2)
			{
				if (Game.Version >= Versions.Beta1)
				{
					packet.PutShort(0);
					packet.PutShort(0);
					packet.PutShort(0); // Status Effects?
					packet.PutByte(0);  // Status Effects?
				}

				packet.PutByte((byte)character.ClassId);
				packet.PutByte((byte)character.Sex);
				packet.AddPackedPosition(character.Position, character.Direction);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte((byte)character.HairId);
				packet.PutByte((byte)character.WeaponId);
				packet.PutByte((byte)character.HeadTopId);
				packet.PutByte((byte)character.State);
			}
			else
			{
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort((short)character.ClassId);
				packet.PutShort((short)character.HairId);
				packet.PutShort((short)character.WeaponId);
				packet.PutShort(0);     // Head1
				packet.PutShort(0);     // Shield
				packet.PutShort(0);     // Head2
				packet.PutShort(0);     // Head3
				packet.PutShort(0);     // HairColor
				packet.PutShort(0);     // ClothesColor
				packet.PutShort(0);     // HeadDir
				packet.PutInt(0);       // GuildId?
				packet.PutShort(0);     // GuildEmblemId?
				packet.PutShort(0);     // Manner?
				packet.PutShort(0);     // Karma?
				packet.PutByte(0);      // BattleStance?
				packet.PutByte((byte)character.Sex);
				packet.AddPackedPosition(character.Position, character.Direction);
				packet.PutByte(0);      // ?
				packet.PutByte(0);      // ?
				packet.PutByte((byte)character.State);

				if (Game.Version >= Versions.EP4)
					packet.PutShort(0); // ?
			}
		}

		/// <summary>
		/// Adds stand entry information about character to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="character"></param>
		public static void AddStandEntryNpc(this Packet packet, IEntryCharacter character)
		{
			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);

			if (Game.Version < Versions.EP4)
			{
				if (Game.Version >= Versions.Beta1)
				{
					packet.PutShort(0);
					packet.PutShort(0);
					packet.PutShort(0); // Status Effects?
					packet.PutByte(0);  // Status Effects?
				}

				packet.PutByte((byte)character.ClassId);
				packet.PutByte((byte)character.Sex);
				packet.AddPackedPosition(character.Position, character.Direction);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte((byte)character.HairId);
				packet.PutByte((byte)character.WeaponId);
				packet.PutByte((byte)character.HeadTopId);
			}
			else
			{
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort((short)character.ClassId);
				packet.PutShort((short)character.HairId);
				packet.PutShort((short)character.WeaponId);
				packet.PutShort(0);     // Head1?
				packet.PutShort(0);     // Shield?
				packet.PutShort(0);     // Head2?
				packet.PutShort(0);     // Head3?
				packet.PutShort(0);     // HairColor?
				packet.PutShort(0);     // ClothesColor?
				packet.PutShort(0);     // HeadDir?
				packet.PutByte(0);      // BattleStance?
				packet.PutByte((byte)character.Sex);
				packet.AddPackedPosition(character.Position, character.Direction);
				packet.PutByte(0);      // ?
				packet.PutByte(0);      // ?
			}
		}

		/// <summary>
		/// Adds new entry information about character to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="character"></param>
		public static void AddNewEntry(this Packet packet, IEntryCharacter character)
		{
			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);

			if (Game.Version < Versions.EP3_2)
			{
				if (Game.Version >= Versions.Beta1)
				{
					packet.PutShort(0);
					packet.PutShort(0);
					packet.PutShort(0); // Status Effects?
					packet.PutByte(0);  // Status Effects?
				}

				packet.PutByte((byte)character.ClassId);
				packet.PutByte((byte)character.Sex);
				packet.AddPackedPosition(character.Position, character.Direction);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutByte((byte)character.HairId);
				packet.PutByte((byte)character.WeaponId);
				packet.PutByte((byte)character.HeadTopId);
			}
			else
			{
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort((short)character.ClassId);
				packet.PutShort((short)character.HairId);
				packet.PutShort((short)character.WeaponId);
				packet.PutShort(0);     // Head1
				packet.PutShort(0);     // Shield
				packet.PutShort(0);     // Head2
				packet.PutShort(0);     // Head3
				packet.PutShort(0);     // HairColor
				packet.PutShort(0);     // ClothesColor
				packet.PutShort(0);     // HeadDir
				packet.PutInt(0);       // GuildId?
				packet.PutShort(0);     // GuildEmblemId?
				packet.PutShort(0);     // Manner?
				packet.PutShort(0);     // Karma?
				packet.PutByte(0);      // BattleStance?
				packet.PutByte((byte)character.Sex);
				packet.AddPackedPosition(character.Position, character.Direction);
				packet.PutByte(0);      // ?
				packet.PutByte(0);      // ?

				if (Game.Version >= Versions.EP4)
					packet.PutShort(0); // ?
			}
		}

		/// <summary>
		/// Adds stand entry information about character to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void AddMoveEntry(this Packet packet, IEntryCharacter character, Position from, Position to)
		{
			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);

			if (Game.Version < Versions.EP3_2)
			{
				if (Game.Version >= Versions.Beta1)
				{
					packet.PutShort(0);
					packet.PutShort(0);
					packet.PutShort(0);
					packet.PutByte(0);
				}

				packet.PutByte((byte)character.ClassId);
				packet.PutByte((byte)character.Sex);
				packet.AddPackedMove(from, to, 8, 8);
				packet.PutShort(0);
				packet.PutByte((byte)character.HairId);
				packet.PutByte((byte)character.WeaponId);
				packet.PutByte(0);
				packet.PutInt(DateTime.Now);
			}
			else
			{
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort(0);     // Status Effects?
				packet.PutShort((short)character.ClassId);
				packet.PutShort((short)character.HairId);
				packet.PutShort((short)character.WeaponId);
				packet.PutShort(0);     // Head1
				packet.PutInt(DateTime.Now);
				packet.PutShort(0);     // Shield
				packet.PutShort(0);     // Head2
				packet.PutShort(0);     // Head3
				packet.PutShort(0);     // HairColor
				packet.PutShort(0);     // ClothesColor
				packet.PutShort(0);     // HeadDir
				packet.PutInt(0);       // GuildId?
				packet.PutShort(0);     // GuildEmblemId?
				packet.PutShort(0);     // Manner?
				packet.PutShort(0);     // Karma?
				packet.PutByte(0);      // BattleStance?
				packet.PutByte((byte)character.Sex);
				packet.AddPackedMove(from, to, 8, 8);
				packet.PutByte(0);      // ?
				packet.PutByte(0);      // ?

				if (Game.Version >= Versions.EP4)
					packet.PutShort(0); // ?
			}
		}
	}
}
