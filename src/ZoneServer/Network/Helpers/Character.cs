using Sabine.Shared;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
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
		public static void AddCharacterEntry(this Packet packet, IEntryCharacter character)
		{
			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);

			if (Game.Version < Versions.EP5)
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
				packet.PutByte(0);      // State
				packet.PutByte(0);      // ?
			}
		}
	}
}
