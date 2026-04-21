using Sabine.Shared;
using Sabine.Shared.Network;
using Sabine.Zone.Skills;

namespace Sabine.Zone.Network.Helpers
{
	/// <summary>
	/// Extensions for adding skill information to packets.
	/// </summary>
	public static class PacketSkillExtensions
	{
		/// <summary>
		/// Adds the skill's information to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="skill"></param>
		public static void AddSkill(this Packet packet, Skill skill)
		{
			// The alpha client has some handlers for skill packets, but
			// they're limited to information about the skills. There are
			// no usage packets and the client doesn't even display the
			// skills sent to it. Still, the version checks here allow
			// us to send the skill info without issues.

			packet.PutShort((short)skill.Id);

			if (Game.Version >= Versions.Beta1)
			{
				packet.PutShort((short)skill.Data.TargetType);
				packet.PutShort(0);
			}

			packet.PutShort((short)skill.Level);
			packet.PutShort((short)skill.SpCost);

			if (Game.Version >= Versions.Beta1)
			{
				packet.PutShort((short)skill.Range);
			}

			packet.PutString(skill.Data.StringId, Sizes.SkillNames);
			packet.PutByte(skill.CanBeLeveled);
		}
	}
}
