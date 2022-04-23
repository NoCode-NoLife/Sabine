namespace Sabine.Zone.World.Entities.CharacterComponents
{
	/// <summary>
	/// Represents a character's parameters (stats, sub-stats, and
	/// anything related).
	/// </summary>
	public class NpcParameters : Parameters<Npc>
	{
		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="character"></param>
		public NpcParameters(Npc character)
			: base(character)
		{
			this.Speed = 400;
		}
	}
}
