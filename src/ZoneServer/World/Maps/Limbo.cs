using Sabine.Shared.Data.Databases;
using Sabine.Zone.World.Actors;

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
		public override void AddPlayer(PlayerCharacter character)
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="character"></param>
		public override void RemovePlayer(PlayerCharacter character)
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

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="item"></param>
		public override void AddItem(Item item)
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="item"></param>
		public override void RemoveItem(Item item)
		{
		}
	}
}
