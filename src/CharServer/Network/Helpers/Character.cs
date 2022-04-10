using Sabine.Char.Database;
using Sabine.Shared.Network;

namespace Sabine.Char.Network.Helpers
{
	public static class PacketCharacterExtensions
	{
		public static void AddCharacter(this Packet packet, Character character)
		{
			packet.PutInt(character.Id);
			packet.PutInt(character.BaseExp);
			packet.PutInt(character.Zeny);
			packet.PutInt(character.JobExp);
			packet.PutInt(0); // ?
			packet.PutInt(0); // ?
			packet.PutInt(0); // ?
			packet.PutShort((short)character.StatPoints);
			packet.PutShort((short)character.Hp);
			packet.PutShort((short)character.HpMax);
			packet.PutShort((short)character.Sp);
			packet.PutShort((short)character.SpMax);
			packet.PutShort((short)character.Speed);
			packet.PutShort(0); // Karma
			packet.PutShort(0); // Manner
			packet.PutString(character.Name, 16);
			packet.PutByte((byte)character.JobId);
			packet.PutByte((byte)character.BaseLevel);
			packet.PutByte((byte)character.JobLevel);
			packet.PutByte((byte)character.Str);
			packet.PutByte((byte)character.Agi);
			packet.PutByte((byte)character.Vit);
			packet.PutByte((byte)character.Int);
			packet.PutByte((byte)character.Dex);
			packet.PutByte((byte)character.Luk);
			packet.PutByte((byte)character.Slot);
			packet.PutByte(0); // Gap
			packet.PutByte((byte)character.HairId);
			packet.PutByte((byte)character.WeaponId);
			packet.PutByte(0); // ?
		}
	}
}
