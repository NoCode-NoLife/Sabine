using System.Threading;

namespace Sabine.Zone.World.Entities
{
	public class Item
	{
		private static int HandlePool = 0x0500_0000;

		public int Handle { get; set; }
		public string StringId { get; set; }
		public int EquipSlot { get; set; }
		public int Type { get; set; }
		public int Amount { get; set; }

		public Item(string stringId)
		{
			this.Handle = GetNewHandle();
			this.StringId = stringId;
		}

		/// <summary>
		/// Returns a new handle.
		/// </summary>
		/// <returns></returns>
		private static int GetNewHandle()
			=> Interlocked.Increment(ref HandlePool);
	}
}
