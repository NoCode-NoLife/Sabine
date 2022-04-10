using System;
using Sabine.Shared.Const;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Maps;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a player character.
	/// </summary>
	public class PlayerCharacter
	{
		public ZoneConnection Connection { get; set; } = new DummyConnection();
		public string Username => this.Connection.Account.Username;
		public int SessionId => this.Connection.Account.SessionId;

		public int Id { get; set; }
		public string Name { get; set; } = "exec";

		public Sex Sex { get; set; } = Sex.Male;
		public JobId JobId { get; set; } = JobId.Thief;

		public string MapName { get; set; } = "prt_vilg01";
		public Position Position { get; set; } = new Position(100, 80);

		public int Speed { get; set; } = 200;

		public int HairId { get; set; } = 0;
		public int WeaponId { get; set; } = 0;

		public int Str { get; set; } = 1;
		public int Agi { get; set; } = 1;
		public int Vit { get; set; } = 1;
		public int Int { get; set; } = 1;
		public int Dex { get; set; } = 1;
		public int Luk { get; set; } = 1;
		public int StatPoints { get; set; } = 10;

		public int StrNeeded => (1 + (this.Str + 9) / 10);
		public int AgiNeeded => (1 + (this.Agi + 9) / 10);
		public int VitNeeded => (1 + (this.Vit + 9) / 10);
		public int IntNeeded => (1 + (this.Int + 9) / 10);
		public int DexNeeded => (1 + (this.Dex + 9) / 10);
		public int LukNeeded => (1 + (this.Luk + 9) / 10);

		public int AtkMin { get; set; } = 10;
		public int AtkMax { get; set; } = 15;
		public int Matk { get; set; } = 1;
		public int Defense { get; set; } = 7;

		public int Weight { get; set; } = 10;
		public int WeightMax { get; set; } = 8000;

		public int SkillPoints { get; set; } = 10;

		public int BaseExp { get; set; } = 10;
		public int JobExp { get; set; } = 5;
		public int BaseExpNeeded { get; set; } = 150;
		public int JobExpNeeded { get; set; } = 75;

		public int BaseLevel { get; set; } = 1;
		public int JobLevel { get; set; } = 1;

		/// <summary>
		/// Returns a reference to the map the character is currently on.
		/// </summary>
		public Map Map { get; set; } = Map.Limbo;

		/// <summary>
		/// Sends a server message to the character's client that is
		/// displayed in the chat log.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void ServerMessage(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			var message = string.Format(Localization.Get("[Server] : {0}"), format);

			Send.ZC_NOTIFY_CHAT(this, 0, message);
		}

		/// <summary>
		/// Returns the value for the given stat.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int GetStat(ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Str: return this.Str;
				case ParameterType.Agi: return this.Agi;
				case ParameterType.Vit: return this.Vit;
				case ParameterType.Int: return this.Int;
				case ParameterType.Dex: return this.Dex;
				case ParameterType.Luk: return this.Luk;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
		}

		/// <summary>
		/// Modifies the given stat and updates the client.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int ModifyStat(ParameterType type, int modifier)
		{
			if (type < ParameterType.Str || type > ParameterType.Luk)
				throw new ArgumentException($"Invalid stat type '{type}'.");

			var newValue = 0;
			switch (type)
			{
				case ParameterType.Str: newValue = this.Str += modifier; break;
				case ParameterType.Agi: newValue = this.Agi += modifier; break;
				case ParameterType.Vit: newValue = this.Vit += modifier; break;
				case ParameterType.Int: newValue = this.Int += modifier; break;
				case ParameterType.Dex: newValue = this.Dex += modifier; break;
				case ParameterType.Luk: newValue = this.Luk += modifier; break;
			}

			Send.ZC_PAR_CHANGE(this, type, newValue);

			return newValue;
		}

		/// <summary>
		/// Returns the amount of stat points needed to increase the
		/// given stat by one.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int GetStatPointsNeeded(ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Str: return this.StrNeeded;
				case ParameterType.Agi: return this.AgiNeeded;
				case ParameterType.Vit: return this.VitNeeded;
				case ParameterType.Int: return this.IntNeeded;
				case ParameterType.Dex: return this.DexNeeded;
				case ParameterType.Luk: return this.LukNeeded;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
		}

		/// <summary>
		/// Modifies the character's stat points and updates the client.
		/// </summary>
		/// <param name="modifier"></param>
		public void ModifyStatPoints(int modifier)
		{
			this.StatPoints += modifier;

			Send.ZC_PAR_CHANGE(this, ParameterType.StatPoints, this.StatPoints);
		}
	}
}
