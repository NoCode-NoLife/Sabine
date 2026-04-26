using System;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.World;
using Sabine.Zone.World.Maps;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Actors
{
	/// <summary>
	/// Represents an item, either inside a player's inventory or on a map.
	/// </summary>
	public class Item : IActor
	{
		private static int HandlePool = 600_000_000;

		/// <summary>
		/// Returns the id the item is identified by in the data.
		/// </summary>
		public int ClassId => this.Data.ClassId;

		/// <summary>
		/// Returns the item's handle, which identifies it on a map.
		/// </summary>
		public int Handle { get; set; }

		/// <summary>
		/// Gets or sets the item's inventory id, which identifies it
		/// inside an inventory.
		/// </summary>
		public int InventoryId { get; set; }

		/// <summary>
		/// Returns the item's string id, which it's identified by
		/// on early clients, instead of using the class id.
		/// </summary>
		public string StringId { get; private set; }

		/// <summary>
		/// Gets or sets the item's type, which affects where it appears
		/// inside an inventory and what can be done with it.
		/// </summary>
		public ItemType Type => this.Data.Type;

		/// <summary>
		/// Gets or sets on which equip slots the item can be equipped on.
		/// </summary>
		public EquipSlots WearSlots => this.Data.WearSlots;

		/// <summary>
		/// Gets or sets on which slots the item is currently equipped.
		/// </summary>
		public EquipSlots EquippedOn { get; set; }

		/// <summary>
		/// Gets or sets whether the item has been identified.
		/// </summary>
		public bool IsIdentified { get; set; } = true;

		/// <summary>
		/// Returns true if the item is currently equipped.
		/// </summary>
		public bool IsEquipped => this.EquippedOn != EquipSlots.None;

		/// <summary>
		/// Gets or sets the item's amount.
		/// </summary>
		public int Amount
		{
			get => _amount;
			set => _amount = Math2.Clamp(0, short.MaxValue, value);
		}
		private int _amount = 1;

		/// <summary>
		/// Gets or sets the id of the map the item is on, if any.
		/// </summary>
		public int MapId { get; set; }

		/// <summary>
		/// Gets or sets the item's position on the map it's on.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets a reference to the map the item is on.
		/// </summary>
		public Map Map { get; set; }

		/// <summary>
		/// Returns true if this item is stackable and can have an
		/// amount greater than 1.
		/// </summary>
		public bool IsStackable => !this.Type.IsEquip();

		/// <summary>
		/// Returns a reference to the item's data entry.
		/// </summary>
		public ItemData Data { get; private set; }

		/// <summary>
		/// Returns a reference to the item's name data (if there is any).
		/// </summary>
		public ItemNameData NameData { get; private set; }

		/// <summary>
		/// Returns the time at which this item should be removed from
		/// the map it was dropped on.
		/// </summary>
		public DateTime DropDisappearTime { get; private set; } = DateTime.MaxValue;

		/// <summary>
		/// Creates new item from class id.
		/// </summary>
		/// <param name="classId"></param>
		public Item(int classId, int amount = 1)
		{
			this.Handle = GetNewHandle();
			this.Amount = Math.Max(1, amount);

			this.LoadData(classId);
		}

		/// <summary>
		/// Returns a new handle.
		/// </summary>
		/// <returns></returns>
		private static int GetNewHandle()
			=> Interlocked.Increment(ref HandlePool);

		/// <summary>
		/// Loads data by class id.
		/// </summary>
		/// <param name="classId"></param>
		/// <exception cref="ArgumentException"></exception>
		private void LoadData(int classId)
		{
			if (!ZoneServer.Instance.Data.Items.TryFind(classId, out var data))
				throw new ArgumentException($"Item with class id '{classId}' not found in database.");

			this.LoadData(data);
		}

		/// <summary>
		/// Loads the given data.
		/// </summary>
		/// <param name="data"></param>
		private void LoadData(ItemData data)
		{
			this.Data = data;

			// This solution isn't ideal, since it's very inflexible.
			// However, there's only two known clients available that
			// require string ids (Alpha and Beta1), and this is a
			// simple solution to getting the correct strings to those
			// two clients.
			if (ZoneServer.Instance.Data.ItemNames.TryFind(this.Data.ClassId, out var nameData))
			{
				this.NameData = nameData;

				if (Game.Version < Versions.Beta1)
					this.StringId = this.NameData.AlphaName ?? "Apple";
				else
					this.StringId = this.NameData.BetaName ?? "Apple";
			}
			// For <= Beta1 we fall back to Apple to prevent crashes
			else if (Game.Version <= Versions.Beta1)
			{
				this.StringId = "Apple";
			}
			else
			{
				this.StringId = this.Data.Name;
			}
		}

		/// <summary>
		/// Adds item to the map, dropping it at the given position.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="pos"></param>
		public void Drop(Map map, Position pos)
		{
			var disappearSeconds = ZoneServer.Instance.Conf.World.ItemDisappearTime;

			this.MapId = map.Id;
			this.Position = pos;
			this.EquippedOn = EquipSlots.None;
			this.DropDisappearTime = DateTime.Now.AddSeconds(disappearSeconds);

			map.AddItem(this);
		}

		/// <summary>
		/// Returns the valid equip slots for this item on the given
		/// character.
		/// </summary>
		/// <remarks>
		/// Effectively nullifies the wear slots if the character can't
		/// equip the item, which prevents the client from showing the
		/// item as equippable. Affects only alpha clients.
		/// </remarks>
		/// <param name="character"></param>
		/// <returns></returns>
		public EquipSlots GetSlotsFor(PlayerCharacter character)
		{
			var wearSlots = this.WearSlots;

			// Disable equipping by setting slots to None, because the
			// alpha client doesn't react to equip fail packets, locking
			// up the client. On newer versions we can leave the slots as
			// is and gracefully decline in the packet handler.
			if (Game.Version < Versions.Beta1)
			{
				if (!character.CanEquip(this))
					wearSlots = EquipSlots.None;
			}

			return wearSlots;
		}
	}
}
