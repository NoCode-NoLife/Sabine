using Sabine.Shared.Data.Databases;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.World.Maps
{
	/// <summary>
	/// A dummy map that is used in characters so they never have
	/// a null map that could cause exceptions.
	/// </summary>
	public class Limbo : Map
	{
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public Limbo()
			: base(new MapsData() { Id = 0, StringId = "__limbo__", Name = "Limbo" })
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void LoadData()
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="character"></param>
		public override void AddCharacter(PlayerCharacter character)
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="character"></param>
		public override void RemoveCharacter(PlayerCharacter character)
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="npc"></param>
		public override void AddNpc(Npc npc)
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="npc"></param>
		public override void RemoveNpc(Npc npc)
		{
		}
	}
}
