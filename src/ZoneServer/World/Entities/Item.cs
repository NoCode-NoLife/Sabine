using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sabine.Zone.World.Entities
{
	public class Item
	{
		public int Id { get; set; }
		public string NameId { get; set; }
		public int EquipSlot { get; set; }
		public int Type { get; set; }
		public int Amount { get; set; }

		public Item(string nameId)
		{
			this.NameId = nameId;
		}
	}
}
