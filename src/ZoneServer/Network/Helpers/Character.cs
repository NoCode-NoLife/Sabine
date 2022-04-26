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

			if (Game.Version >= Versions.Beta1)
			{
				packet.PutShort(0);
				packet.PutShort(0);
				packet.PutShort(0); // status effect?
				packet.PutByte(0);  // status effect?
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
	}
}
