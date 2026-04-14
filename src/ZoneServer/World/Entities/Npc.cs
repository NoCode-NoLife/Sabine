using System;
using System.Threading;
using Sabine.Shared;
using Sabine.Shared.World;
using Sabine.Zone.Compatibility;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Entities.Components.Characters;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a non-player character.
	/// </summary>
	public class Npc : Character
	{
		private static int HandlePool = 400_000_000;

		/// <summary>
		/// Returns the NPC's unique handle.
		/// </summary>
		public override int Handle { get; protected set; }

		/// <summary>
		/// Returns the NPC's class id, defining its look.
		/// </summary>
		public override int ClassId { get; protected set; }

		/// <summary>
		/// Gets or sets the function called when a dialog with this
		/// NPC is initiated.
		/// </summary>
		public DialogFunc DialogFunc { get; set; }

		/// <summary>
		/// Gets or sets the NPC's trigger area.
		/// </summary>
		/// <remarks>
		/// The trigger area is a rectangle centered on the NPC. If it's
		/// not empty (meaning both width and height are greater than 0),
		/// the server will check for characters entering and leaving the
		/// area and call the NPC's appropriate functions.
		/// </remarks>
		public TriggerArea TriggerArea { get; set; }

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		/// <param name="classId"></param>
		public Npc(int classId)
		{
			this.Handle = GetNewHandle();
			this.ClassId = classId;
			this.DisplayClassId = CharacterClasses.GetDisplayClassId(Game.Version, classId);

			this.Parameters = new NpcParameters(this);

			this.Direction = Direction.South;
			this.Parameters.Speed = 400;
		}

		/// <summary>
		/// Returns a new handle.
		/// </summary>
		/// <returns></returns>
		private static int GetNewHandle()
			=> Interlocked.Increment(ref HandlePool);

		/// <summary>
		/// Warps NPC to the given position.
		/// </summary>
		/// <param name="location"></param>
		/// <exception cref="ArgumentException"></exception>
		public override void Warp(Location location)
		{
			if (!ZoneServer.Instance.World.Maps.TryGet(location.MapId, out var newMap))
				throw new ArgumentException($"Map '{location.MapId}' not found.");

			var curMap = this.Map;

			this.MapId = location.MapId;
			this.Position = location.Position;

			curMap.RemoveNpc(this);
			newMap.AddNpc(this);
		}
	}

	/// <summary>
	/// Defines a rectangular area around a character that can trigger
	/// functions when other character enter or leave it.
	/// </summary>
	/// <remarks>
	/// The trigger area is a rectangle centered on the character. With a
	/// range of 0 on both axes, a character will trigger the area when
	/// they step on the same tile as the owner. With a range of 1, the
	/// area will also trigger when a character is on any of the 8 tiles
	/// surrounding the owner. With a range of 2, it will trigger on 2
	/// tiles in all directions from the owner, and so on.
	/// </remarks>
	/// <param name="owner">The owner of the area, on which the area is centered.</param>
	/// <param name="rangeX">The horizontal range of the trigger area.</param>
	/// <param name="rangeY">The vertical range of the trigger area.</param>
	public class TriggerArea(Character owner, int rangeX, int rangeY)
	{
		/// <summary>
		/// Returns the character this trigger area belongs to.
		/// </summary>
		public Character Owner { get; } = owner;

		/// <summary>
		/// Gets or sets the width of the trigger area.
		/// </summary>
		public int RangeX { get; set; } = rangeX;

		/// <summary>
		/// Gets or sets the height of the trigger area.
		/// </summary>
		public int RangeY { get; set; } = rangeY;

		/// <summary>
		/// Gets or sets a function called when a character enters the
		/// trigger area.
		/// </summary>
		public TriggerFunc Enter { get; set; }

		/// <summary>
		/// Gets or sets a function called when a character leaves the
		/// trigger area.
		/// </summary>
		public TriggerFunc Exit { get; set; }

		/// <summary>
		/// Returns true if the given position is inside the trigger area.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool Contains(Position pos)
		{
			var ownerPos = this.Owner.Position;
			var rangeX = this.RangeX;
			var rangeY = this.RangeY;

			if (pos.X < ownerPos.X - rangeX || pos.X > ownerPos.X + rangeX)
				return false;

			if (pos.Y < ownerPos.Y - rangeY || pos.Y > ownerPos.Y + rangeY)
				return false;

			return true;
		}
	}

	/// <summary>
	/// Represents a method that handles a trigger event involving two
	/// characters.
	/// </summary>
	/// <param name="character">The character that triggered the event.</param>
	/// <param name="other">The owner of the trigger area.</param>
	public delegate void TriggerFunc(Character character, Character other);
}
