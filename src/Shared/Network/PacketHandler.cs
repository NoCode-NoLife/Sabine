using System;
using System.Collections.Generic;
using Yggdrasil.Logging;

namespace Sabine.Shared.Network
{
	/// <summary>
	/// Packet handler delegate manager.
	/// </summary>
	/// <typeparam name="TConnection"></typeparam>
	public class PacketHandler<TConnection>
	{
		public delegate void PacketHandlerFunc(TConnection conn, Packet packet);

		protected Dictionary<int, PacketHandlerFunc> _handlers = new Dictionary<int, PacketHandlerFunc>();

		/// <summary>
		/// Creates new packet handler and loads handler methods on itself.
		/// </summary>
		public PacketHandler()
		{
			this.LoadMethods();
		}

		/// <summary>
		/// Loads methods with the PacketHandlerAttribute inside this class.
		/// </summary>
		public void LoadMethods()
		{
			foreach (var method in this.GetType().GetMethods())
			{
				foreach (PacketHandlerAttribute attr in method.GetCustomAttributes(typeof(PacketHandlerAttribute), false))
				{
					var func = (PacketHandlerFunc)Delegate.CreateDelegate(typeof(PacketHandlerFunc), this, method);
					foreach (var op in attr.Ops)
						this.Add(op, func);
				}
			}
		}

		/// <summary>
		/// Sets handler for the given op.
		/// </summary>
		/// <param name="op"></param>
		/// <param name="func"></param>
		public void Add(int op, PacketHandlerFunc func)
		{
			_handlers[op] = func;
		}

		/// <summary>
		/// Handles the given packet for the connection.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		/// <returns></returns>
		public bool Handle(TConnection conn, Packet packet)
		{
			if (!_handlers.TryGetValue(packet.Op, out var func))
			{
				Log.Debug("PacketHandler: No handler found for 0x{0:X4} ({1}).\r\n{2}", packet.Op, PacketTable.GetName(packet.Op), packet.ToString());
				return false;
			}

			func(conn, packet);
			return true;
		}
	}

	/// <summary>
	/// Methods with this attribute may be loaded as handlers automatically
	/// via "LoadMethods".
	/// </summary>
	public class PacketHandlerAttribute : Attribute
	{
		/// <summary>
		/// Returns the opcodes this handler handles.
		/// </summary>
		public int[] Ops { get; private set; }

		/// <summary>
		/// Creates new attribute.
		/// </summary>
		/// <param name="ops"></param>
		public PacketHandlerAttribute(params int[] ops)
		{
			this.Ops = ops;
		}
	}
}
