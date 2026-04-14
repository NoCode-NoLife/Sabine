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
			packet.PutShort((short)skill.Id);
			packet.PutShort((short)skill.Data.TargetType);
			packet.PutShort(0);
			packet.PutShort((short)skill.Level);
			packet.PutShort((short)skill.SpCost);
			packet.PutShort((short)skill.Range);
			packet.PutString(skill.Data.StringId, 24);
			packet.PutByte(skill.CanBeLeveled);
		}
	}
}
