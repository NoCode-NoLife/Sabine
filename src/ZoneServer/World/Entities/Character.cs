using System;
using Sabine.Shared.Const;
using Sabine.Shared.World;
using Sabine.Zone.World.Entities.Components.Characters;
using Sabine.Zone.World.Maps;
using Shared.Const;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// A character that can interact with the world and can be
	/// interacted with, such as a player or an NPC.
	/// </summary>
	public abstract class Character : IEntryCharacter, IUpdateable
	{
		/// <summary>
		/// Returns the character's unique handle, which it's identified
		/// as during interactions with the world and other characters.
		/// </summary>
		public abstract int Handle { get; protected set; }

		/// <summary>
		/// Returns the character's name.
		/// </summary>
		public virtual string Name { get; set; } = "";

		/// <summary>
		/// Returns the character's username.
		/// </summary>
		public virtual string Username { get; } = "";

		/// <summary>
		/// Returns the character's class id, defining (part of) its
		/// appearance.
		/// </summary>
		public abstract int ClassId { get; protected set; }

		/// <summary>
		/// Returns the character's sex.
		/// </summary>
		public virtual Sex Sex { get; }

		/// <summary>
		/// Returns a character's hair id, defining the look of their
		/// head.
		/// </summary>
		public virtual int HairId { get; set; }

		/// <summary>
		/// Returns a character's weapon id, defining what weapon they
		/// can be seen holding during combat.
		/// </summary>
		public virtual int WeaponId { get; set; }

		/// <summary>
		/// Returns a character's current state.
		/// </summary>
		public virtual CharacterState State { get; set; }

		/// <summary>
		/// Gets or sets the id of the map the character is currently on.
		/// </summary>
		public int MapId { get; set; } = 100036;

		/// <summary>
		/// Gets or sets the character's a reference to the map the
		/// character is currently on.
		/// </summary>
		public Map Map
		{
			get => _map;
			internal set => _map = value ?? Map.Limbo;
		}
		private Map _map = Map.Limbo;

		/// <summary>
		/// Gets or sets the character's current position.
		/// </summary>
		public Position Position { get; set; } = new Position(99, 81);

		/// <summary>
		/// Returns the direction the character is turned towards.
		/// </summary>
		public virtual Direction Direction { get; set; } = Direction.North;

		/// <summary>
		/// Returns the character's speed parameter.
		/// </summary>
		public int Speed => this.Parameters.Speed;

		/// <summary>
		/// Returns the character's movement controller.
		/// </summary>
		public MovementController Controller { get; protected set; }

		/// <summary>
		/// Returns the character's parameters.
		/// </summary>
		public Parameters Parameters { get; protected set; }

		/// <summary>
		/// Returns the character's components.
		/// </summary>
		public CharacterComponents Components { get; } = new CharacterComponents();

		/// <summary>
		/// Returns true if the character's HP have reached 0.
		/// </summary>
		public bool IsDead => this.Parameters.Hp == 0;

		/// <summary>
		/// Initializes character.
		/// </summary>
		public Character()
		{
			this.Components.Add(this.Controller = new MovementController(this));
		}

		/// <summary>
		/// Updates the character's components.
		/// </summary>
		/// <param name="elapsed"></param>
		public virtual void Update(TimeSpan elapsed)
		{
			this.Components.Update(elapsed);
		}

		/// <summary>
		/// Warps character to the given location.
		/// </summary>
		/// <param name="location"></param>
		public abstract void Warp(Location location);

		/// <summary>
		/// Reduces the character's HP by the given amount, returns the
		/// character's remaining HP.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="attacker"></param>
		/// <returns></returns>
		public virtual int TakeDamage(int amount, Character attacker)
		{
			var remainingHp = this.Parameters.Modify(ParameterType.Hp, -amount);

			if (remainingHp == 0)
				this.Kill(attacker);

			return remainingHp;
		}

		/// <summary>
		/// Kills the character.
		/// </summary>
		/// <param name="killer"></param>
		public virtual void Kill(Character killer)
		{
			// TODO: Figure out what needs to happen when we kill
			//   different kinds of entities.

			this.Parameters.Hp = 0;
			this.State = CharacterState.Dead;
		}

		/// <summary>
		/// Drops item in range of the character.
		/// </summary>
		/// <param name="item"></param>
		public void Drop(Item item)
		{
			var pos = this.Position.GetRandomInSquareRange(1);
			item.Drop(this.Map, pos);
		}
	}
}
