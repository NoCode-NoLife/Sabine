using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabine.Shared.Network
{
	/// <summary>
	/// Manages a list of packet opcodes and their lengths.
	/// </summary>
	public static partial class PacketTable
	{
		public static void Load()
		{
			LoadVersion100();

			if (Game.Version >= Versions.Beta1)
				LoadVersion200();

			if (Game.Version >= Versions.Beta2)
				LoadVersion300();

			if (Game.Version >= Versions.EP3)
				LoadVersion400();

			if (Game.Version >= Versions.EP4)
				LoadVersion500();

			BuildLists();
		}

		/// <summary>
		/// Numeric value indicating a dynamic packet size.
		/// </summary>
		public const int Dynamic = -1;

		private static readonly List<PacketTableEntry> Entries = new List<PacketTableEntry>();
		private static readonly Dictionary<int, int> Sizes = new Dictionary<int, int>();
		private static readonly Dictionary<int, string> Names = new Dictionary<int, string>();
		private static readonly Dictionary<int, Op> NetworkToHost = new Dictionary<int, Op>();
		private static readonly Dictionary<Op, int> HostToNetwork = new Dictionary<Op, int>();

		/// <summary>
		/// Adds a new packet to the table.
		/// </summary>
		/// <param name="op">The op code the packet will be know as internally.</param>
		/// <param name="opNetwork">The op code that is sent/received by the client.</param>
		/// <param name="size">The size of the packet (-1 for dynamic).</param>
		/// <param name="shift">Whether to shift the following ops if the new one is inserted in between others.</param>
		/// <exception cref="ArgumentException"></exception>
		private static void Register(Op op, int opNetwork, int size, bool shift = true)
		{
			var newEntry = new PacketTableEntry(op, opNetwork, size);

			if (Entries.Count == 0)
			{
				Entries.Add(newEntry);
				return;
			}

			var existing = Entries.FirstOrDefault(a => a.Op == op);
			if (existing != null)
				throw new ArgumentException($"Op {op} was already added.");

			if (opNetwork > Entries[Entries.Count - 1].OpNetwork)
			{
				Entries.Add(newEntry);
				return;
			}

			var startIndex = -1;
			for (var i = 0; i < Entries.Count; i++)
			{
				if (Entries[i].OpNetwork >= opNetwork)
				{
					startIndex = i;
					Entries.Insert(i, newEntry);
					break;
				}
			}

			if (!shift)
				return;

			if (startIndex == -1)
				throw new ArgumentException("You have done the impossible. Please report.");

			for (var i = startIndex + 1; i < Entries.Count; i++)
				Entries[i].OpNetwork += 1;
		}

		/// <summary>
		/// Changes a packet's size.
		/// </summary>
		/// <param name="op"></param>
		/// <param name="size"></param>
		/// <exception cref="ArgumentException"></exception>
		private static void ChangeSize(Op op, int size)
		{
			var existing = Entries.FirstOrDefault(a => a.Op == op);
			if (existing == null)
				throw new ArgumentException($"Op {op} doesn't exist yet.");

			existing.Size = size;
		}

		/// <summary>
		/// Shifts all opcodes by the given amount.
		/// </summary>
		/// <param name="offset"></param>
		private static void ShiftAll(int offset)
		{
			for (var i = 0; i < Entries.Count; i++)
				Entries[i].OpNetwork += offset;
		}

		/// <summary>
		/// Builds quick access lists for the packed table.
		/// </summary>
		private static void BuildLists()
		{
			// Q: OMG! This is terrible! You're wasting *bytes* of memory!
			//    How *could* you!? You should do X!!!
			// A: If your X has the same performance as this solution,
			//    we can talk, but until then, I think we're fine with
			//    a few KB of wasted memory.

			foreach (var entry in Entries)
			{
				NetworkToHost[entry.OpNetwork] = entry.Op;
				HostToNetwork[entry.Op] = entry.OpNetwork;
				Names[entry.OpNetwork] = entry.Op.ToString();
				Sizes[entry.OpNetwork] = entry.Size;
			}
		}

		/// <summary>
		/// Returns the network op for the given op.
		/// </summary>
		/// <param name="op"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static int ToNetwork(Op op)
		{
			if (!HostToNetwork.TryGetValue(op, out var opNetwork))
				throw new ArgumentException($"Op {op} not found.");

			return opNetwork;
		}

		/// <summary>
		/// Returns the op for the given network op.
		/// </summary>
		/// <param name="opNetwork"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Op ToHost(int opNetwork)
		{
			if (!NetworkToHost.TryGetValue(opNetwork, out var op))
				throw new ArgumentException($"Op '0x{opNetwork:X4}' not found.");

			return op;
		}

		/// <summary>
		/// Returns the size of packets with the given opcode. If size is
		/// -1 (PacketTable.Dynamic), the packet's size is dynamic.
		/// </summary>
		/// <param name="op"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static int GetSize(int op)
		{
			if (!Sizes.TryGetValue(op, out var size))
				throw new ArgumentException($"No size found for op '0x{op:X4}'.");

			return size;
		}

		/// <summary>
		/// Returns the name of the given opcode.
		/// </summary>
		/// <param name="op"></param>
		/// <returns></returns>
		public static string GetName(int op)
		{
			if (!Names.TryGetValue(op, out var name))
				return "?";

			return name;
		}

		/// <summary>
		/// Returns a list of all opcodes, indexed by their name.
		/// </summary>
		/// <returns></returns>
		public static List<PacketTableEntry> GetTable()
			=> Entries;

		/// <summary>
		/// Represents a packet in the packet table.
		/// </summary>
		public class PacketTableEntry
		{
			/// <summary>
			/// Returns the opcode that the packet is identified by on
			/// the server.
			/// </summary>
			public Op Op { get; }

			/// <summary>
			/// Gets or sets the numeric opcode that the packet is
			/// identified by when it's sent/received by the client.
			/// </summary>
			public int OpNetwork { get; set; }

			/// <summary>
			/// Gets or sets the packet's size.
			/// </summary>
			public int Size { get; set; }

			/// <summary>
			/// Creates new entry.
			/// </summary>
			/// <param name="op"></param>
			/// <param name="opNetwork"></param>
			/// <param name="size"></param>
			public PacketTableEntry(Op op, int opNetwork, int size)
			{
				this.Op = op;
				this.OpNetwork = opNetwork;
				this.Size = size;
			}
		}
	}
}
