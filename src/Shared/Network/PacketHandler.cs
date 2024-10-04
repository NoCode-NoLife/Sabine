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
		protected Dictionary<Op, PacketHandlerFunc> _handlers = new Dictionary<Op, PacketHandlerFunc>();

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
		public void Add(Op op, PacketHandlerFunc func)
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
				var opNetwork = PacketTable.ToNetwork(packet.Op);
				Log.Debug("PacketHandler: No handler found for 0x{0:X4} ({1}).\r\n{2}", opNetwork, packet.Op.ToString(), packet.ToString());
				return false;
			}

			func(conn, packet);
			return true;
		}

		/// <summary>
		/// A function that handle a packet.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		public delegate void PacketHandlerFunc(TConnection conn, Packet packet);
	}

	/// <summary>
	/// Methods with this attribute may be loaded as handlers automatically
	/// via "LoadMethods".
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class PacketHandlerAttribute : Attribute
	{
		/// <summary>
		/// Returns the opcodes this handler handles.
		/// </summary>
		public Op[] Ops { get; private set; }

		/// <summary>
		/// Creates new attribute.
		/// </summary>
		/// <param name="ops"></param>
		public PacketHandlerAttribute(params Op[] ops)
		{
			this.Ops = ops;
		}
	}
}
