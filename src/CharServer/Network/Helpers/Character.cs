using Sabine.Char.Database;
using Sabine.Shared;
using Sabine.Shared.Data;
using Sabine.Shared.Network;

namespace Sabine.Char.Network.Helpers
{
	/// <summary>
	/// Character related extensions for the Packet class.
	/// </summary>
	public static class PacketCharacterExtensions
	{
		/// <summary>
		/// Writes character's information to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="character"></param>
		public static void AddCharacter(this Packet packet, Character character)
		{
			var jobLevel = character.JobLevel;
			var jobExp = character.JobExp;

			// Always display job level and EXP as 0 if the feature
			// isn't enabled
			if (!CharServer.Instance.Data.Features.IsEnabled("JobLevels"))
			{
				jobLevel = 0;
				jobExp = 0;
			}

			packet.PutInt(character.Id);
			packet.PutInt(character.BaseExp);
			packet.PutInt(character.Zeny);
			packet.PutInt(jobExp);
			packet.PutInt(jobLevel); // Job Level in Beta1
			packet.PutInt(0); // ?
			packet.PutInt(0); // status effects bitmask?

			if (Game.Version >= Versions.Beta1)
			{
				packet.PutInt(0); // ?
				packet.PutInt(0); // Karma?
				packet.PutInt(0); // Manner?
			}

			packet.PutShort((short)character.StatPoints);
			packet.PutShort((short)character.Hp);
			packet.PutShort((short)character.HpMax);
			packet.PutShort((short)character.Sp);
			packet.PutShort((short)character.SpMax);
			packet.PutShort((short)character.Speed);

			if (Game.Version < Versions.Beta1)
			{
				packet.PutShort(0); // Karma?
				packet.PutShort(0); // Manner?
			}

			if (Game.Version < Versions.Beta2)
			{
				packet.PutString(character.Name, Sizes.CharacterNames);
				packet.PutByte((byte)character.JobId);
				packet.PutByte((byte)character.BaseLevel);
				packet.PutByte((byte)jobLevel); // Job Level in Alpha
				packet.PutByte((byte)character.Str);
				packet.PutByte((byte)character.Agi);
				packet.PutByte((byte)character.Vit);
				packet.PutByte((byte)character.Int);
				packet.PutByte((byte)character.Dex);
				packet.PutByte((byte)character.Luk);
				packet.PutByte((byte)character.Slot);
				packet.PutByte(0); // Gap
				packet.PutByte((byte)character.HairId);
				packet.PutByte((byte)character.WeaponLook);
				packet.PutByte((byte)character.HeadBottomLook);
			}
			else
			{
				packet.PutShort((short)character.JobId);
				packet.PutShort((short)character.HairId);
				packet.PutShort((short)character.WeaponLook);
				packet.PutShort((short)character.BaseLevel);
				packet.PutShort(0); // SkillPoints
				packet.PutShort((short)character.HeadBottomLook);
				packet.PutShort(0); // ShieldId
				packet.PutShort((short)character.HeadTopLook);
				packet.PutShort((short)character.HeadMiddleLook);
				packet.PutShort((short)character.HairColorId);
				packet.PutShort(0); // ClothesColorId
				packet.PutString(character.Name, Sizes.CharacterNames);
				packet.PutByte((byte)character.Str);
				packet.PutByte((byte)character.Agi);
				packet.PutByte((byte)character.Vit);
				packet.PutByte((byte)character.Int);
				packet.PutByte((byte)character.Dex);
				packet.PutByte((byte)character.Luk);
				packet.PutByte((byte)character.Slot);
				packet.PutByte(0); // Gap
			}
		}
	}
}
